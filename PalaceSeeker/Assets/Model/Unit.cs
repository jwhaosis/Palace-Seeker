﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Unit {

    public static string unitLayer = "Unit";
    public static string actionLayer = "UnitTiles";

    World map;
    int x;
    int y;
    bool selected;
    GameObject unitObject;

    int movement;
    HashSet<Tile> movementSquares;

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
    public Unit(World map, int x, int y, UnitController parent) {
        Debug.Log("Unit created at " + x + "," + y + ".");
        this.map = map;
        this.x = x;
        this.y = y;
        this.movementSquares = new HashSet<Tile>();

        GameObject unitObject = new GameObject();
        unitObject.AddComponent<SpriteRenderer>().sortingLayerName = unitLayer;
        unitObject.transform.SetParent(parent.transform);
        unitObject.transform.position = new Vector3(x, y, 0);
        unitObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Units/FoxSprite");

        this.unitObject = unitObject;
        this.Selected = false;

        movement = 3;

    }

    public void MoveTo(int x, int y) {
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not move out of map.");
        }
        if (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type)) {
            Debug.Log("Can not move over invalid tiles.");
        }
        if (movementSquares.Contains(map.GetTile(x, y))) {
            map.UnitArray[this.x, this.y] = null;
            map.UnitArray[x, y] = this;
            this.x = x;
            this.y = y;
            unitObject.transform.position = new Vector3(x, y, 0);
        } else {
            Debug.Log("Out of movement range.");
        }
    }

    public void GenerateMovementGrid(bool currentlySelected) {
        if (currentlySelected) {
            movementSquares.Add(map.GetTile(x, y));
            ShowMovement(x,y, movement);
        } else {
            movementSquares.Clear();
            foreach (Transform child in this.unitObject.transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void ShowMovement(int x, int y, int moveRemaining) {
        if (moveRemaining <= 0) {
            return;
        } else {
            moveRemaining--;
        }
        if (y < map.Height - 1) {
            Tile tile1 = map.GetTile(x, y + 1);
            ShowMovementVisuals(tile1, moveRemaining);
        }
        if (x < map.Width - 1) {
            Tile tile2 = map.GetTile(x + 1, y);
            ShowMovementVisuals(tile2, moveRemaining);
        }
        if (y > 0) {
            Tile tile3 = map.GetTile(x, y - 1);
            ShowMovementVisuals(tile3, moveRemaining);
        }
        if (x > 0) {
            Tile tile4 = map.GetTile(x - 1, y);
            ShowMovementVisuals(tile4, moveRemaining);
        }

    }

    private void ShowMovementVisuals(Tile tempTile, int moveRemaining) {
        if (!Tile.unreachableTypes.Contains(tempTile.Type)) {
            if (movementSquares.Add(tempTile)) {
                GameObject newTile = new GameObject();
                newTile.transform.SetParent(this.unitObject.transform, true);
                newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/MovementTile");
                newTile.GetComponent<SpriteRenderer>().sortingLayerName = actionLayer;
                newTile.transform.position = new Vector3(tempTile.X, tempTile.Y, 0);
            }
            ShowMovement(tempTile.X, tempTile.Y, moveRemaining);
        }
    }
}
