using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator {

    World map;
    int tileCount;

    public MapGenerator(World map) {
        this.map = map;
    }
	
    public void Generate() {
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (Random.Range(0, 2) == 0) {
                    map.TileArray[x, y].Type = Tile.TileType.Floor;
                }
                else {
                    map.TileArray[x, y].Type = Tile.TileType.Wall;
                }
            }
        }
        if (!MapValid()) {
            Generate();
        }

    }

    private bool MapValid() {
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (Reachable(map.TileArray[x, y])) {
                    tileCount++;
                } else {
                    map.TileArray[x, y].Type = Tile.TileType.Wall;
                }
            }
        }
        return !MapUndersized();
    }

    private bool MapUndersized() {
        if (tileCount < 300) {
            Debug.Log("Tilecount is " + tileCount + ", map is undersized.");
            return true;
        }
        else {
            Debug.Log("Tilecount is " + tileCount + ", map is functional.");
            return false;
        }
    }

    public bool Reachable(Tile tile) {
        int[] surroundingTiles = SurroundingTiles(tile);
        if (!Tile.unreachableTypes.Contains(tile.Type)) {
            return surroundingTiles.Contains(4);
        }
        else {
            return FillDiagonals(tile, surroundingTiles);
        }
    }

    public int[] SurroundingTiles(Tile tile) {
        //index 0 is top, 1 is right, 2 is bot, 3 is left
        //value 0 is on edge, 1 is unreachable, 4 is reachable
        int[] surroundingTiles = new int[4];
        surroundingTiles[0] = AdjacentTileCode(tile.X, tile.Y+1);
        surroundingTiles[1] = AdjacentTileCode(tile.X+1, tile.Y);
        surroundingTiles[2] = AdjacentTileCode(tile.X, tile.Y-1);
        surroundingTiles[3] = AdjacentTileCode(tile.X-1, tile.Y);
        return surroundingTiles;
    }

   private bool FillDiagonals(Tile tile, int[] surroundingTiles) {
        bool edited = false;
        if(surroundingTiles[0]==1 && surroundingTiles[1] == 1) {
            if(!Tile.unreachableTypes.Contains(map.GetTile(tile.X + 1, tile.Y + 1).Type)) {
                if (Random.Range(0, 2) == 0) {
                    map.GetTile(tile.X + 1, tile.Y).Type = Tile.TileType.Floor;
                } else {
                    map.GetTile(tile.X, tile.Y + 1).Type = Tile.TileType.Floor;
                }
                tileCount++;
                edited = true;
            }
        }
        if (surroundingTiles[0] == 1 && surroundingTiles[3] == 1) {
            if (!Tile.unreachableTypes.Contains(map.GetTile(tile.X - 1, tile.Y + 1).Type)) {
                if (Random.Range(0, 2) == 0) {
                    map.GetTile(tile.X - 1, tile.Y).Type = Tile.TileType.Floor;
                }
                else {
                    map.GetTile(tile.X, tile.Y + 1).Type = Tile.TileType.Floor;
                }
                tileCount++;
                edited = true;
            }
        }
        if (surroundingTiles[2] == 1 && surroundingTiles[1] == 1) {
            if (!Tile.unreachableTypes.Contains(map.GetTile(tile.X + 1, tile.Y - 1).Type)) {
                if (Random.Range(0, 2) == 0) {
                    map.GetTile(tile.X + 1, tile.Y).Type = Tile.TileType.Floor;
                }
                else {
                    map.GetTile(tile.X, tile.Y - 1).Type = Tile.TileType.Floor;
                }
                tileCount++;
                edited = true;
            }
        }
        if (surroundingTiles[2] == 1 && surroundingTiles[3] == 1) {
            if (!Tile.unreachableTypes.Contains(map.GetTile(tile.X - 1, tile.Y - 1).Type)) {
                if (Random.Range(0, 2) == 0) {
                    map.GetTile(tile.X - 1, tile.Y).Type = Tile.TileType.Floor;
                }
                else {
                    map.GetTile(tile.X, tile.Y - 1).Type = Tile.TileType.Floor;
                }
                tileCount++;
                edited = true;
            }
        }
        return edited;
    }

    private int AdjacentTileCode(int x, int y) {
        if(x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            return 0;
        }
        Tile tile = map.GetTile(x, y);
        if (Tile.unreachableTypes.Contains(tile.Type)) {
            return 1;
        } else {
            return 4;
        }
    }

}
