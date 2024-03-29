﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    private static WorldController _instance;
    private static string layer = "Tile";

    public Sprite floorSprite;
    public Sprite wallSprite;
    public Sprite waterSprite;


    World map;

    public static WorldController Instance {
        get {
            return _instance;
        }
    }

    public World World {
        get {
            return map;
        }
    }

    private void Awake() {
        _instance = this;
    }

    void Start() {
        map = new World();
        //create visuals for tiles
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {

                Tile tile_data = map.GetTile(x, y);
                GameObject tile_go = new GameObject {
                    name = "Tile_" + x + "_" + y
                };

                GameObject fog_go = new GameObject {
                    name = "Fog_" + x + "_" + y
                };

                tile_go.AddComponent<SpriteRenderer>().sortingLayerName = layer;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                fog_go.AddComponent<SpriteRenderer>().sortingLayerName = "FogOfWar";
                fog_go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/FogOfWar");
                fog_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                fog_go.transform.SetParent(this.transform, true);
                fog_go.AddComponent<BoxCollider2D>();

                //add visual rendering to onTileTypeChange in tile class
                tile_data.AddOnTileTypeChangeAction((tile) => { ChangeTileSprite(tile, tile_go); });
            }
        }
        map.GenerateWorld();
        UnitController.Instance.Initialize(map);
    }


    // Update is called once per frame
    void Update() {
    }

    void ChangeTileSprite(Tile tile_data, GameObject tile_go) {
        if (tile_data.Type == Tile.TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        } else if (tile_data.Type == Tile.TileType.Wall) {
            tile_go.GetComponent<SpriteRenderer>().sprite = waterSprite;
        } else if (tile_data.Type == Tile.TileType.Water) {
            tile_go.GetComponent<SpriteRenderer>().sprite = waterSprite;
        }
    }

    public Tile GetTileAtCoordinate (Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return map.GetTile(x, y);
    }
}
