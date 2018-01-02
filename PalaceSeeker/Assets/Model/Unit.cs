using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Unit {

    World map;
    int x;
    int y;
    bool selected;
    GameObject unitObject;

    int movement;
    

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
            GenerateMovementGrid(selected);
        }
    }

    //methods--------------------------------------------------
    public Unit(World map, int x, int y, GameObject unitObject) {
        Debug.Log("Unit created at " + x + "," + y + ".");
        this.map = map;
        this.x = x;
        this.y = y;
        this.Selected = false;
        this.unitObject = unitObject;

        movement = 1;

    }

    public bool MoveTo(int x, int y) {
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not move out of map.");
            return false;
        }
        if (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type)) {
            Debug.Log("Can not move over invalid tiles.");
            return false;
        }
        map.UnitArray[this.x, this.y] = null;
        map.UnitArray[x, y] = this;
        this.x = x;
        this.y = y;
        unitObject.transform.position = new Vector3(x, y, 0);
        return true;
    }

    private void GenerateMovementGrid(bool currentlySelected) {
        if (currentlySelected) {
            ShowMovement(movement);
        } else {

        }
    }

    private void ShowMovement(int moveRemaining) {
        if (moveRemaining <= 0) {
            return;
        }
        

    }

}
