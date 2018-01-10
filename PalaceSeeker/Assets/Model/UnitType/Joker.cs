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

    public override void SpecialOneGrid() {
        if (Selected) {
            ClearAllGrids();
            GenerateRange(x, y, 3, UnitCommands.Special, specialSquares);
            GenerateVisuals("Tiles/SpecialTile", specialSquares);
        }
        else {
            ClearAllGrids();
            TurnFinished = true;
        }
    }

    public override bool SpecialOne(int x, int y) {
        if (x == this.x && y == this.y) {
            return false;
        }
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not buff outside of map.");
            return false;
        }
        else if (specialSquares.Contains(map.GetTile(x, y))) {
            Unit targetUnit = map.GetUnit(x, y);
            if (targetUnit == null) {
                Debug.Log("Can not buff an unoccupied square.");
                return false;
            }
            else if (targetUnit.controller != this.controller) {
                Debug.Log("Can not buff enemy units.");
                return false;
            }
            else {
                Debug.Log("Buffing unit at " + x + ", " + y + ".");
                targetUnit.ChangeUnitStats(UnitStats.Movement, 2, 0);
                SpecialOneGrid();
                return true;
            }
        }
        else {
            Debug.Log("Out of buff range.");
            return false;
        }
    }
}