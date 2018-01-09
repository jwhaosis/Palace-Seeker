using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//MapGenerator1 but with an extra check for unreachable tiles using a hashset
public class MapGenerator2 {

    World map;
    World emptyMap;

    public MapGenerator2(World map) {
        this.map = map;
        this.emptyMap = map;
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
            this.map = this.emptyMap;
            Generate();
        }

    }

    private bool MapValid() {
        for (int x = 0; x < map.Width; x++) {
            for (int y = 0; y < map.Height; y++) {
                if (!Reachable(map.TileArray[x, y])) {
                    map.TileArray[x, y].Type = Tile.TileType.Wall;
                }
            }
        }
        return TravelMap();
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
        surroundingTiles[0] = AdjacentTileCode(tile.X, tile.Y + 1);
        surroundingTiles[1] = AdjacentTileCode(tile.X + 1, tile.Y);
        surroundingTiles[2] = AdjacentTileCode(tile.X, tile.Y - 1);
        surroundingTiles[3] = AdjacentTileCode(tile.X - 1, tile.Y);
        return surroundingTiles;
    }

    private bool FillDiagonals(Tile tile, int[] surroundingTiles) {
        bool edited = false;
        if (surroundingTiles[0] == 1 && surroundingTiles[1] == 1) {
            if (!Tile.unreachableTypes.Contains(map.GetTile(tile.X + 1, tile.Y + 1).Type)) {
                if (Random.Range(0, 2) == 0) {
                    map.GetTile(tile.X + 1, tile.Y).Type = Tile.TileType.Floor;
                }
                else {
                    map.GetTile(tile.X, tile.Y + 1).Type = Tile.TileType.Floor;
                }
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
                edited = true;
            }
        }
        return edited;
    }

    private int AdjacentTileCode(int x, int y) {
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            return 0;
        }
        Tile tile = map.GetTile(x, y);
        if (Tile.unreachableTypes.Contains(tile.Type)) {
            return 1;
        }
        else {
            return 4;
        }
    }

    //iterate through map to see if it has unreachable rooms
    private bool TravelMap() {
        int x = Random.Range(0, map.Width);
        int y = Random.Range(0, map.Height);
        Tile tile = map.GetTile(x, y);
        HashSet<Tile> reachableTiles = new HashSet<Tile> { tile };
        TravelOne(x, y, ref reachableTiles);
        for (int a = 0; a < map.Width; a++) {
            for (int b = 0; b < map.Height; b++) {
                if (!reachableTiles.Contains(map.GetTile(a, b))) {
                    map.GetTile(a, b).Type = Tile.TileType.Wall;
                }
            }
        }
        return !MapUndersized(reachableTiles.Count);
    }

    private bool MapUndersized(int tileCount) {
        if (tileCount < 250) {
            return true;
        }
        else {
            return false;
        }
    }


    private void TravelOne(int x, int y, ref HashSet<Tile> reachableTiles) {
        if (y < map.Height - 1) {
            Tile tile1 = map.GetTile(x, y + 1);
            if (!Tile.unreachableTypes.Contains(tile1.Type) && reachableTiles.Add(tile1)) {
                TravelOne(tile1.X, tile1.Y, ref reachableTiles);
            }
            
        }
        if (x < map.Width - 1) {
            Tile tile2 = map.GetTile(x + 1, y);
            if (!Tile.unreachableTypes.Contains(tile2.Type) && reachableTiles.Add(tile2)) {
                TravelOne(tile2.X, tile2.Y, ref reachableTiles);
            }

        }
        if (y > 0) {
            Tile tile3 = map.GetTile(x, y - 1);
            if (!Tile.unreachableTypes.Contains(tile3.Type) && reachableTiles.Add(tile3)) {
                TravelOne(tile3.X, tile3.Y, ref reachableTiles);
            }

        }
        if (x > 0) {
            Tile tile4 = map.GetTile(x - 1, y);
            if (!Tile.unreachableTypes.Contains(tile4.Type) && reachableTiles.Add(tile4)) {
                TravelOne(tile4.X, tile4.Y, ref reachableTiles);
            }

        }

    }

}
