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

}