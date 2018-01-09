using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Unit {

    private static string sprite = "Units/JokerSprite";

    public Joker(World map, int x, int y) : base(map, x, y, sprite) {
        unitObject.name = "Joker";
        unitType = "Joker";

        movement = 2;
        rangeMin = 1;
        rangeMax = 1;
        health = 10;
        attack = 5;
        defense = 5;
    }

    public override void SpecialOne() {
        if (Selected) {
            ClearAllGrids();
            specialSquares.Add(map.GetTile(x, y));
            GetAdjacentSquares(x, y, 2, 1, "Tiles/SpecialTile", specialSquares);
        }
        else {
            ClearAllGrids();
            TurnFinished = true;
        }
    }

}