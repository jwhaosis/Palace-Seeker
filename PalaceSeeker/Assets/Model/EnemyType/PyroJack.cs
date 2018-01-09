using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroJack : Enemy {

    static string sprite = "Units/Enemies/PyroJack";

    public PyroJack(World map, int x, int y) : base(map, x, y, sprite) {
        movement = 2;
        rangeMin = 1;
        rangeMax = 1;
        health = 15;
        attack = 10;
        defense = 0;
    }

}
