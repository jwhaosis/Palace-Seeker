using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player {

    public int playerNum;
    private Leader leader = null;
    private List<Unit> unitList;

    //getters and setters------------------------------
    public Leader Leader {
        get {
            return leader;
        }
    }


    //methods------------------------------
    public Player(int playerNum) {
        this.playerNum = playerNum;
        this.unitList = new List<Unit>();
    }

    public void RefreshUnits() {
        foreach(Unit unit in unitList) {
            unit.Refresh();
        }
    }

    public void ChangeAllUnitStats(Unit.UnitStats stat, int change, int duration) {
        foreach (Unit unit in unitList) {
            unit.ChangeUnitStats(stat, change, duration);
            Debug.Log(unit.unitType + " gains " + change + " " + stat + " from leader bonuses.");
        }
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
        unitList.Add(unit);
        Debug.Log("Unit added as unit " + unitList.Count);
        return true;
    }

    public bool RemoveUnit(Unit unit) {
        unitList.Remove(unit);
        Debug.Log("Unit removed.");
        return true;
    }


    public bool IsCompleteUnits() {
        return unitList.Count == 2;
    }

    public bool IsCompleteAll() {
        return IsCompleteUnits() && leader != null;
    }


}
