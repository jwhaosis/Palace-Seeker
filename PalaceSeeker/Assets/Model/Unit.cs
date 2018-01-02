using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit {

    World map;
    int x;
    int y;
    bool selected;

    //getters and setters------------------------------------------------
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

    //methods----------------------------------------------------
    public Unit(World map, int x, int y) {
        Debug.Log("Unit created at " + x + "," + y + ".");
        this.map = map;
        this.x = x;
        this.y = y;
        this.selected = false;
    }

    public bool MoveTo(int x, int y) {
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not move out of map.");
            return false;
        }

        map.UnitArray[this.x, this.y] = null;
        map.UnitArray[x, y] = this;
        this.x = x;
        this.y = y;
        return true;
    }

}
