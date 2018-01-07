using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroJack : Enemy {

    static string sprite = "Units/Enemies/PyroJack";

    public PyroJack(World map, int x, int y, UnitController parent) : base(map, x, y, sprite, parent) {
        movement = 2;
        rangeMin = 1;
        rangeMax = 1;
        health = 2;
        attack = 1;
        defense = 1;
    }

}
