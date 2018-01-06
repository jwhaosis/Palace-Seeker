using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Unit {

    private static string sprite = "Units/FoxSprite";

    public Fox(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        movement = 4;
        health = 8;
        attack = 7;
        defense = 4;
    }

}
