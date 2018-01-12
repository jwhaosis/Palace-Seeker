using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panther : Unit {

    private static string sprite = "Units/PantherSprite";

    public Panther(World map, int x, int y) : base(map, x, y, sprite) {
        unitObject.name = "Panther";
        unitType = "Panther";

        movement = 6;
        rangeMin = 2;
        rangeMax = 2;
        health = 5;
        attack = 10;
        defense = 2;
    }

    public override void SpecialOneGrid() {
        GenerateGrid(4, UnitCommands.Special);
    }

    public override bool SpecialOne(int x, int y) {
        if (x == this.x && y == this.y) {
            return false;
        }
        if (x >= map.Width || x < 0 || y >= map.Height || y < 0) {
            Debug.Log("Can not attack outside of map.");
            return false;
        }
        else if (specialSquares.Contains(map.GetTile(x, y))) {
            Unit targetUnit = map.GetUnit(x, y);
            if (targetUnit == null) {
                Debug.Log("Can not attack an unoccupied square.");
                return false;
            }
            else if (targetUnit.controller == this.controller) {
                Debug.Log("Can not attack friendly units.");
                return false;
            }
            else {
                Debug.Log("Attacked " + x + ", " + y + ".");
                Combat thisCombat = new Combat(this, targetUnit);
                thisCombat.CalculateCombat();
                TurnFinished = true;
                return true;
            }
        }
        else {
            Debug.Log("Out of attack range.");
            return false;
        }
    }
}