using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class Unit {

    protected static string unitLayer = "Unit";
    protected static string actionLayer = "UnitTiles";

    protected World map;
    protected int x;
    protected int y;
    protected bool selected;
    protected bool moved;
    protected bool turnFinished;

    protected GameObject unitObject;
    protected HashSet<Tile> movementSquares;
    protected HashSet<Tile> attackSquares;
    public string unitType;

    protected int movement;
    protected int rangeMin;
    protected int rangeMax;
    protected int health;
    protected int attack;
    protected int defense;

    //getters and setters--------------------------------------------------
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
            GenerateMovementGrid(value);
        }
    }

    public bool Moved {
        get {
            return moved;
        }

        set {
            moved = value;
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

    //methods--------------------------------------------------

    protected Unit(World map, int x, int y, string sprite, UnitController parent) {
        this.map = map;
        this.x = x;
        this.y = y;
        this.movementSquares = new HashSet<Tile>();
        this.attackSquares = new HashSet<Tile>();

        GameObject unitObject = new GameObject();
        unitObject.AddComponent<SpriteRenderer>().sortingLayerName = unitLayer;
        unitObject.transform.SetParent(parent.transform);
        unitObject.transform.position = new Vector3(x, y, 0);
        unitObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);

        this.unitObject = unitObject;
        this.Selected = false;
        this.moved = false;
        this.turnFinished = false;
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
                GenerateMovementGrid(false);
                moved = true;
                return true;
            }
        } else {
            Debug.Log("Out of movement range.");
            return false;
        }
    }

    public bool Attack(int x, int y) {
        if (x == this.x && y == this.y) {
            return false;
        }
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not attack outside of map.");
            return false;
        }
        else if (attackSquares.Contains(map.GetTile(x, y))) {
            if (map.GetUnit(x, y) == null) {
                Debug.Log("Can not attack an unoccupied square.");
                GenerateAttackGrid(false);
                return true;
            }
            else {
                Debug.Log("Attacked " + x + ", " + y + ".");
                GenerateAttackGrid(false);
                return true;
            }
        }
        else {
            Debug.Log("Out of attack range.");
            return false;
        }
    }


    public void GenerateMovementGrid(bool currentlySelected) {
        if (currentlySelected) {
            movementSquares.Add(map.GetTile(x, y));
            GetAdjacentSquares(x, y, movement, 0, "Tiles/MovementTile", movementSquares);
        } else {
            movementSquares.Clear();
            foreach (Transform child in this.unitObject.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void GenerateAttackGrid(bool currentlySelected) {
        if (currentlySelected) {
            attackSquares.Add(map.GetTile(x, y));
            GetAdjacentSquares(x, y, rangeMax, rangeMin, "Tiles/AttackTile", attackSquares);
        }
        else {
            movementSquares.Clear();
            foreach (Transform child in this.unitObject.transform) {
                GameObject.Destroy(child.gameObject);
            }
            turnFinished = true;
        }
    }

    private void GetAdjacentSquares(int x, int y, int moveRemaining, int minRange, string sprite, HashSet<Tile> tileArray) {
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

    private void ShowVisuals(Tile tempTile, int moveRemaining, int minRange, string sprite, HashSet<Tile> tileArray) {
        if (!Tile.unreachableTypes.Contains(tempTile.Type)) {
            if (tileArray.Add(tempTile) && minRange<=0) {
                GameObject newTile = new GameObject();
                newTile.transform.SetParent(this.unitObject.transform, true);
                newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);
                newTile.GetComponent<SpriteRenderer>().sortingLayerName = actionLayer;
                newTile.transform.position = new Vector3(tempTile.X, tempTile.Y, 0);
            }
            GetAdjacentSquares(tempTile.X, tempTile.Y, moveRemaining, minRange, sprite, tileArray);
        }
    }

    public void ChangeUnitStats(string stat, int change) {
        this.movement += change;
    }

    public void Delete() {
        Debug.Log("Unit GameObject Removed.");
        this.map.UnitArray[this.x, this.y] = null;
        GameObject.Destroy(unitObject);
    }
}
