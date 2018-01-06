using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panther : Unit {

    private static string sprite = "Units/PantherSprite";

    public Panther(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        movement = 6;
        health = 5;
        attack = 10;
        defense = 2;
    }

}