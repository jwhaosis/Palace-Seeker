using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit{

    public Enemy(World map, int x, int y, string sprite, UnitController parent) : base(map, x, y, sprite, parent) {
        this.unitType = "enemy";
    }
}
