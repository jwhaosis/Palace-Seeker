using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class Unit {

    protected static string unitLayer = "Unit";
    protected static string actionLayer = "UnitTiles";
    public enum UnitStats { Health, Attack, Defense, Movement };

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
    protected Player controller;
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
                GenerateAttackGrid();
                return true;
            } else if (targetUnit.controller == this.controller) {
                Debug.Log("Can not attack friendly units.");
                return false;
            }
            else {
                Debug.Log("Attacked " + x + ", " + y + ".");
                Combat thisCombat = new Combat(this, map.GetUnit(x, y));
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

    public abstract void SpecialOne();

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
            GetAdjacentSquares(x, y, this.Movement, 0, "Tiles/MovementTile", movementSquares);
            GenerateSelect();
        } else {
            ClearAllGrids();
        }
    }

    public void GenerateAttackGrid() {
        if (Selected) {
            ClearAllGrids();
            attackSquares.Add(map.GetTile(x, y));
            GetAdjacentSquares(x, y, rangeMax, rangeMin, "Tiles/AttackTile", attackSquares);
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

    protected void GetAdjacentSquares(int x, int y, int moveRemaining, int minRange, string sprite, HashSet<Tile> tileArray) {
        if (moveRemaining <= 0) {
            return;
        } else {
            minRange--;
            moveRemaining--;
        }
        if (y < map.Height - 1) {
            Tile tile1 = map.GetTile(x, y + 1);
            ShowVisuals(tile1, moveRemaining, minRange, sprite, tileArray);
        }
        if (x < map.Width - 1) {
            Tile tile2 = map.GetTile(x + 1, y);
            ShowVisuals(tile2, moveRemaining, minRange, sprite, tileArray);
        }
        if (y > 0) {
            Tile tile3 = map.GetTile(x, y - 1);
            ShowVisuals(tile3, moveRemaining, minRange, sprite, tileArray);
        }
        if (x > 0) {
            Tile tile4 = map.GetTile(x - 1, y);
            ShowVisuals(tile4, moveRemaining, minRange, sprite, tileArray);
        }

    }

    protected void ShowVisuals(Tile tempTile, int moveRemaining, int minRange, string sprite, HashSet<Tile> tileArray) {
        if(sprite.Contains("Movement") && map.GetUnit(tempTile.X, tempTile.Y) != null && map.GetUnit(tempTile.X, tempTile.Y).controller != this.controller) {
            return;
        }
        else if (!Tile.unreachableTypes.Contains(tempTile.Type)) {
            if ((tileArray.Add(tempTile) && minRange<=0)) {
                GameObject newTile = new GameObject();
                newTile.transform.SetParent(this.unitObject.transform, true);
                newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);
                newTile.GetComponent<SpriteRenderer>().sortingLayerName = actionLayer;
                newTile.transform.position = new Vector3(tempTile.X, tempTile.Y, 0);
            }
            GetAdjacentSquares(tempTile.X, tempTile.Y, moveRemaining, minRange, sprite, tileArray);
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
