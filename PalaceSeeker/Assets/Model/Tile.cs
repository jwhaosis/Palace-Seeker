using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile {
    //location of tile in game
    World map;
    int x;
    int y;

    //data about the tile status
    public enum TileType { Floor, Wall };
    public static TileType[] unreachableTypes = { TileType.Wall };

    TileType type = TileType.Floor;

    Action<Tile> onTileTypeChange;

    //getters and setters------------------------------
    public TileType Type {
        get {
            return type;
        }

        set {
            type = value;
            if (onTileTypeChange != null) {
                onTileTypeChange(this);
            }
        }
    }
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

    //methods------------------------------
    public Tile( World map, int x, int y) {
        this.map = map;
        this.x = x;
        this.y = y;
    }

    public void AddOnTileTypeChangeAction(Action<Tile> action) {
        onTileTypeChange = action;
    }


}
