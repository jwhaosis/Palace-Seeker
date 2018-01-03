using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    private static WorldController _instance;
    private static string layer = "Tile";

    public Sprite floorSprite;

    World world;

    public static WorldController Instance {
        get {
            return _instance;
        }
    }

    public World World {
        get {
            return world;
        }
    }

    void Start() {
        world = new World();
        _instance = this;
        //create visuals for tiles
        for (int x = 0; x < world.Width; x++) {
            for (int y = 0; y < world.Height; y++) {

                Tile tile_data = world.GetTile(x, y);
                GameObject tile_go = new GameObject {
                    name = "Tile_" + x + "_" + y
                };

                tile_go.AddComponent<SpriteRenderer>().sortingLayerName = layer;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);

                //add visual rendering to onTileTypeChange in tile class
                tile_data.AddOnTileTypeChangeAction((tile) => { ChangeTileSprite(tile, tile_go); });
            }
        }

        world.GenerateWorld();
    }


    // Update is called once per frame
    void Update() {
        SummonUnit();
    }

    void ChangeTileSprite(Tile tile_data, GameObject tile_go) {
        if (tile_data.Type == Tile.TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public Tile GetTileAtCoordinate (Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return world.GetTile(x, y);
    }


    //UNIT CONTROLS, PROBABLY PUT INTO PLAYER CONTROLLER LATER
    private void SummonUnit() {
        if (Input.GetKeyDown("space")) {
            UnitController.Instance.CreateUnit(this.world);
        }
    }

}
