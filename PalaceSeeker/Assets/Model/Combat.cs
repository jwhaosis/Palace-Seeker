using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat {

    Unit attackingUnit;
    Unit defendingUnit;
    
    public Combat(Unit attackingUnit, Unit defendingUnit) {
        this.attackingUnit = attackingUnit;
        this.defendingUnit = defendingUnit;
    }

    public void CalculateCombat() {
        int damage = attackingUnit.Attack - defendingUnit.Defense;
        if (damage < 0) {
            damage = 0;
        }
        Debug.Log(defendingUnit.unitType + " has " + defendingUnit.Health + " health. " + attackingUnit.unitType + " attacks for " + damage + " damage.");
        Debug.Log(defendingUnit.unitType + " has " + (defendingUnit.Health - damage) + " health remaining.");
        defendingUnit.ChangeUnitStats(Unit.UnitStats.Health, -damage);
        if(defendingUnit.Health>0) {
            damage = defendingUnit.Attack - attackingUnit.Defense;
            if (damage < 0) {
                damage = 0;
            }
            Debug.Log(attackingUnit.unitType + " has " + attackingUnit.Health + " health. " + defendingUnit.unitType + " retaliates for " + damage + " damage.");
            Debug.Log(attackingUnit.unitType + " has " + (attackingUnit.Health - damage) + " health remaining.");
            attackingUnit.ChangeUnitStats(Unit.UnitStats.Health, -damage);
        }
    }
}
