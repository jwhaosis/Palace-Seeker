using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit{

    //methods------------------------------
    public Enemy(World map, int x, int y, string sprite) : base(map, x, y, sprite) {
        this.unitType = "enemy";
        this.controller = null;
    }

    public override void SpecialOne() {
        return;
    }

}
