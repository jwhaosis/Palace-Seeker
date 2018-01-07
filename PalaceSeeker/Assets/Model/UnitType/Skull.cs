using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Unit {

    private static string sprite = "Units/SkullSprite";

    public Skull(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        unitObject.name = "Skull";
        unitType = "Skull";

        movement = 1;
        rangeMin = 1;
        rangeMax = 1;
        health = 20;
        attack = 6;
        defense = 6;
    }

}