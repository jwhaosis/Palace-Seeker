using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class Unit {

    protected static string unitLayer = "Unit";
    protected static string actionLayer = "UnitTiles";
    public enum UnitStats { Health, Attack, Defense, Movement };
    public enum UnitCommands { Move, Attack, Special };

    protected World map;
    protected int x;
    protected int y;
    protected bool selected;
    protected bool moved;
    protected bool turnFinished;

    protected GameObject unitObject;
    protected HashSet<Tile> movementSquares;
    protected HashSet<Tile> attackSquares;
    protected HashSet<Tile> specialSquares;
    public Player controller;
    public string unitType;

    protected int rangeMin;
    protected int rangeMax;
    protected int health;
    protected int attack;
    protected int[] attackBonuses;
    protected int defense;
    protected int[] defenseBonuses;
    protected int movement;
    protected int[] movementBonuses;


    protected string specialOneName;
    protected string specialTwoName;
    protected string specialThreeName;

    //getters and setters------------------------------
    public int X {
        get {
            return x;
        }
    }
    public int Y {
        get {
            return y;
        }
    }

    public bool Selected {
        get {
            return selected;
        }

        set {
            selected = value;
            GenerateMovementGrid();
        }
    }

    public bool Moved {
        get {
            return moved;
        }

        set {
            moved = value;
            GenerateMovementGrid();
        }
    }

    public bool TurnFinished {
        get {
            return turnFinished;
        }

        set {
            turnFinished = value;
        }
    }

    public int Health {
        set {
            health = value;
            if (health <= 0) {
                this.Delete();
            }
        }
        get {
            return health;
        }
    }

    public int Attack {
        get {
            return GetTotalStats(UnitStats.Attack);
        }
    }

    public int Defense {
        get {
            return GetTotalStats(UnitStats.Defense);
        }
    }

    public int Movement {
        get {
            return GetTotalStats(UnitStats.Movement);
        }
    }


    public Player Controller {
        get {
            return controller;
        }
    }

    //methods------------------------------

    protected Unit(World map, int x, int y, string sprite) {
        this.map = map;
        this.x = x;
        this.y = y;
        this.movementSquares = new HashSet<Tile>();
        this.attackSquares = new HashSet<Tile>();
        this.specialSquares = new HashSet<Tile>();

        GameObject unitObject = new GameObject();
        unitObject.AddComponent<SpriteRenderer>().sortingLayerName = unitLayer;
        unitObject.transform.SetParent(UnitController.Instance.transform);
        unitObject.transform.position = new Vector3(x, y, 0);
        unitObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);

        this.controller = PlayerController.Instance.GetCurrentPlayer();
        this.unitObject = unitObject;
        this.Selected = false;
        this.moved = false;
        this.turnFinished = false;

        this.attackBonuses = new int[1];
        this.defenseBonuses = new int[1];
        this.movementBonuses = new int[1];

    }

    public bool MoveTo(int x, int y) {
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not move out of map.");
            return false;
        }
        else if (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type)) {
            Debug.Log("Can not move over invalid tiles.");
            return false;
        }
        else if (movementSquares.Contains(map.GetTile(x, y))) {
            if (map.GetUnit(x, y) != null && map.GetUnit(x, y) != this) {
                Debug.Log("Can not move to an occupied square.");
                return false;
            }
            else {
                map.UnitArray[this.x, this.y] = null;
                map.UnitArray[x, y] = this;
                this.x = x;
                this.y = y;
                unitObject.transform.position = new Vector3(x, y, 0);
                return true;
            }
        } else {
            Debug.Log("Out of movement range.");
            return false;
        }
    }

    public bool AttackSquare(int x, int y) {
        if (x == this.x && y == this.y) {
            return false;
        }
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not attack outside of map.");
            return false;
        }
        else if (attackSquares.Contains(map.GetTile(x, y))) {
            Unit targetUnit = map.GetUnit(x, y);
            if (targetUnit == null) {
                Debug.Log("Can not attack an unoccupied square.");
                return false;
            } else if (targetUnit.controller == this.controller) {
                Debug.Log("Can not attack friendly units.");
                return false;
            }
            else {
                Debug.Log("Attacked " + x + ", " + y + ".");
                Combat thisCombat = new Combat(this, targetUnit);
                thisCombat.CalculateCombat();
                GenerateAttackGrid();           
                return true;
            }
        }
        else {
            Debug.Log("Out of attack range.");
            return false;
        }
    }

    public abstract bool SpecialOne(int x, int y);

    public abstract void SpecialOneGrid();

    //public abstract void SpecialTwo();

    //public abstract void SpecialThree();

    public void GenerateSelect() {
        if (!Moved) {
            GameObject selectTile = new GameObject { name = "SelectTile" };
            selectTile.transform.SetParent(this.unitObject.transform, true);
            selectTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/SelectTile");
            selectTile.GetComponent<SpriteRenderer>().sortingLayerName = actionLayer;
            selectTile.transform.position = new Vector3(x, y, 0);
        }
    }

    public void GenerateMovementGrid() {
        if (Selected && !Moved) {
            ClearAllGrids();
            movementSquares.Add(map.GetTile(x, y));
            GenerateRange(x, y, this.Movement, UnitCommands.Move, movementSquares);
            GenerateVisuals("Tiles/MovementTile", movementSquares);
            GenerateSelect();
        } else {
            ClearAllGrids();
        }
    }

    public void GenerateAttackGrid() {
        if (Selected) {
            ClearAllGrids();
            attackSquares.Add(map.GetTile(x, y));
            GenerateRange(x, y, rangeMax, UnitCommands.Attack, attackSquares);
            GenerateVisuals("Tiles/AttackTile", attackSquares);
        }
        else {
            ClearAllGrids();
            TurnFinished = true;
        }
    }

    protected void ClearAllGrids() {
        movementSquares.Clear();
        attackSquares.Clear();
        specialSquares.Clear();
        foreach (Transform child in this.unitObject.transform) {
            if (!child.name.Contains("Select") || Selected == false) {
                GameObject.Destroy(child.gameObject);
            }
        }

    }

    protected void GenerateRange(int x, int y, int moveRemaining, UnitCommands commandType, HashSet<Tile> tileArray) {
        if (moveRemaining != 0) {
            moveRemaining--;
            Tile[] surroundingTiles = new Tile[4];
            surroundingTiles[0] = map.GetTile(x, y + 1);
            surroundingTiles[1] = map.GetTile(x + 1, y);
            surroundingTiles[2] = map.GetTile(x, y - 1);
            surroundingTiles[3] = map.GetTile(x - 1, y);
            foreach (Tile tile in surroundingTiles) {
                if(commandType == UnitCommands.Move) {
                    if (tile != null && !Tile.unreachableTypes.Contains(tile.Type)) {
                        if(map.GetUnit(tile.X, tile.Y)==null || map.GetUnit(tile.X, tile.Y).controller == this.controller) {
                            tileArray.Add(tile);
                            GenerateRange(tile.X, tile.Y, moveRemaining, commandType, tileArray);
                        }
                    }
                } else {
                    if (tile != null) {
                        tileArray.Add(tile);
                        GenerateRange(tile.X, tile.Y, moveRemaining, commandType, tileArray);
                    }
                }
            }
        }
    }

    protected void GenerateVisuals(string sprite, HashSet<Tile> tileArray) {
        foreach(Tile tile in tileArray) {
            if(tile.X != this.X || tile.Y != this.Y) {
                GameObject newTile = new GameObject();
                newTile.transform.SetParent(this.unitObject.transform, true);
                newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);
                newTile.GetComponent<SpriteRenderer>().sortingLayerName = actionLayer;
                newTile.transform.position = new Vector3(tile.X, tile.Y, 0);
            }
        }
    }

    public void ChangeUnitStats(Unit.UnitStats stat, int change, int duration) {
        if(stat == UnitStats.Attack) {
            attackBonuses[duration] += change;
        } else if (stat == UnitStats.Defense) {
            defenseBonuses[duration] += change;
        } else if (stat == UnitStats.Movement) {
            movementBonuses[duration] += change;
        } else {
            Debug.Log("Not a valid stat to change.");
        }
    }

    public int GetTotalStats(Unit.UnitStats stat) {
        if(stat == UnitStats.Attack) {
            int totalAttack = this.attack;
            foreach (int buff in attackBonuses) {
                totalAttack += buff;
            }
            return totalAttack;
        } else if (stat == UnitStats.Defense) {
            int totalDefense = this.defense;
            foreach (int buff in defenseBonuses) {
                totalDefense += buff;
            }
            return totalDefense;
        } else if (stat == UnitStats.Movement) {
            int totalMove = this.movement;
            foreach (int buff in movementBonuses) {
                totalMove += buff;
            }
            return totalMove;
        }
        else {
            return 0;
        }
    }

    public void Delete() {
        Debug.Log("Unit GameObject Deleted.");
        this.map.UnitArray[this.x, this.y] = null;
        GameObject.Destroy(unitObject);
        
    }

}
