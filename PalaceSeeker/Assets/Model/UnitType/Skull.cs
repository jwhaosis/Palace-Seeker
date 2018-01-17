using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Unit {

    private static string sprite = "Units/SkullSprite";

    public Skull(World map, int x, int y) : base(map, x, y, sprite) {
        unitObject.name = "Skull";
        unitType = "Skull";

        movement = 1;
        rangeMin = 1;
        rangeMax = 1;
        health = 20;
        attack = 6;
        defense = 6;
    }

    public override void SpecialOneGrid() {
        GenerateGrid(3, UnitCommands.Special);
    }

    public override bool SpecialOne(int x, int y) {
        this.ChangeUnitStats(UnitStats.Attack, 10, 0);
        foreach (Tile tile in specialSquares) {
            Unit target = map.GetUnit(tile.X, tile.Y);
            if (target!=null && target.Controller != this.Controller) {
                Combat thisCombat = new Combat(this, target);
                thisCombat.CalculateCombat();
            }
        }
        this.ChangeUnitStats(UnitStats.Attack, -10, 0);
        TurnFinished = true;
        return true;
    }


}