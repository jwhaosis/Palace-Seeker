using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    Tile[,] tileArray;
    Unit[,] unitArray;

    int width;
    int height;
    int tileCount;

    //constructor
    public World(int width = 20, int height = 20) {
        this.width = width;
        this.height = height;

        this.tileArray = new Tile[width, height];
        this.unitArray = new Unit[width, height];

        for (int x=0; x<width; x++) {
            for(int y=0; y<height; y++) {
                tileArray[x, y] = new Tile(this, x, y);
            }
        }

        Debug.Log("World created with " + (width*height) + " tiles");
    }

    //getters and setters-------------------------------------------------
    public int Width {
        get {
            return width;
        }
    }

    public int Height {
        get {
            return height;
        }
    }

    public Tile[,] TileArray {
        get {
            return tileArray;
        }

        set {
            tileArray = value;
        }
    }

    public Unit[,] UnitArray {
        get {
            return unitArray;
        }

        set {
            unitArray = value;
        }
    }


    //methods----------------------------------------------------
    public Tile GetTile (int x, int y) {
        if (x >= width || x < 0 || y >= height || y < 0) {
            return null;
        }
        return tileArray[x, y];
    }

    public Unit GetUnit(int x, int y) {
        if (x >= width || x < 0 || y >= height || y < 0) {
            return null;
        }
        return unitArray[x, y];
    }

    public void RemoveUnit(int x, int y) {
        if (x >= width || x < 0 || y >= height || y < 0) {
            return;
        }
        UnitArray[x, y] = null;
    }


    public void GenerateWorld() {
        MapGenerator2 floorGenerator = new MapGenerator2(this);
        floorGenerator.Generate();
    }

}
