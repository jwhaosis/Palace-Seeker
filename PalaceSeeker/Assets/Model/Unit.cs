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
    public Unit(World map, int x, int y, GameObject unitObject) {
        Debug.Log("Unit created at " + x + "," + y + ".");
        this.map = map;
        this.x = x;
        this.y = y;
        this.unitObject = unitObject;
        this.movementSquares = new HashSet<Tile>();
        this.Selected = false;

        movement = 3;

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
        if (movementSquares.Contains(map.GetTile(x, y))) {
            map.UnitArray[this.x, this.y] = null;
            map.UnitArray[x, y] = this;
            this.x = x;
            this.y = y;
            unitObject.transform.position = new Vector3(x, y, 0);
            this.Selected = false;
            return true;
        } else {
            Debug.Log("Out of movement range.");
            return false;
        }
    }

    public void GenerateMovementGrid(bool currentlySelected) {
        if (currentlySelected) {
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
            if (!Tile.unreachableTypes.Contains(tile1.Type)) {
                if (movementSquares.Add(tile1)) {
                    GameObject newTile = new GameObject();
                    newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/MovementTile");
                    newTile.transform.position = new Vector3(tile1.X, tile1.Y, 0);
                    newTile.transform.SetParent(this.unitObject.transform, true);
                }
                ShowMovement(tile1.X, tile1.Y, moveRemaining);
            }

        }
        if (x < map.Width - 1) {
            Tile tile2 = map.GetTile(x + 1, y);
            if (!Tile.unreachableTypes.Contains(tile2.Type)) {
                if (movementSquares.Add(tile2)) {
                    GameObject newTile = new GameObject();
                    newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/MovementTile");
                    newTile.transform.position = new Vector3(tile2.X, tile2.Y, 0);
                    newTile.transform.SetParent(this.unitObject.transform, true);
                }
                ShowMovement(tile2.X, tile2.Y, moveRemaining);
            }

        }
        if (y > 0) {
            Tile tile3 = map.GetTile(x, y - 1);
            if (!Tile.unreachableTypes.Contains(tile3.Type)) {
                if (movementSquares.Add(tile3)) {
                    GameObject newTile = new GameObject();
                    newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/MovementTile");
                    newTile.transform.position = new Vector3(tile3.X, tile3.Y, 0);
                    newTile.transform.SetParent(this.unitObject.transform, true);
                }
                ShowMovement(tile3.X, tile3.Y, moveRemaining);
            }

        }
        if (x > 0) {
            Tile tile4 = map.GetTile(x - 1, y);
            if (!Tile.unreachableTypes.Contains(tile4.Type)) {
                if (movementSquares.Add(tile4)) {
                    GameObject newTile = new GameObject();
                    newTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/MovementTile");
                    newTile.transform.position = new Vector3(tile4.X, tile4.Y, 0);
                    newTile.transform.SetParent(this.unitObject.transform, true);
                }
                ShowMovement(tile4.X, tile4.Y, moveRemaining);
            }

        }

    }
}
