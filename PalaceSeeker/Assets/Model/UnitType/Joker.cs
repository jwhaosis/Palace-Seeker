using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Unit {

    private static string sprite = "Units/JokerSprite";

    public Joker(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        movement = 1;
    }

}