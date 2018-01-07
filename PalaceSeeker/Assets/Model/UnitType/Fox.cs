using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Unit {

    private static string sprite = "Units/FoxSprite";

    public Fox(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        unitObject.name = "Fox";
        unitType = "Fox";

        movement = 4;
        rangeMin = 1;
        rangeMax = 1;
        health = 8;
        attack = 7;
        defense = 4;
    }

}
