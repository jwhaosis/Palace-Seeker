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

    public override void SpecialOne() {
        return;
    }
}