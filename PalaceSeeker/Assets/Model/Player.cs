using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player {

    private int playerNum;
    private Leader leader = null;
    private Unit unitOne = null;
    private Unit unitTwo = null;

    public Leader Leader {
        get {
            return leader;
        }
    }

    public Player(int playerNum) {
        this.playerNum = playerNum;
    }

    public void ChangeAllUnitStats(Unit.UnitStats stat, int change) {
        unitOne.ChangeUnitStats(stat, change);
        unitTwo.ChangeUnitStats(stat, change);
    }

    public bool AddLeader(Leader leader) {
        if (this.leader == null) {
            this.leader = leader;
            this.leader.LeaderEffectStage0();
            Debug.Log("Leader added.");
            return true;
        } else {
            Debug.Log("Full on leaders.");
            return false;
        }
    }

    public bool AddUnit(Unit unit) {
        if (this.unitOne == null) {
            this.unitOne = unit;
            Debug.Log("Unit added to unit one.");
            return true;
        } else if (this.unitTwo == null) {
            this.unitTwo = unit;
            Debug.Log("Unit added to unit two.");
            return true;
        } else {
            Debug.Log("Full on units.");
            unit.Delete();
            return false;
        }
    }

    public bool IsCompleteUnits() {
        return unitOne != null && unitTwo != null;
    }

    public bool IsCompleteAll() {
        return unitOne != null && unitTwo != null && leader != null;
    }


}
