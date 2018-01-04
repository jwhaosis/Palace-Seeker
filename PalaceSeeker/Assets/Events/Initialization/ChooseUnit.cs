using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseUnit : MonoBehaviour {

    public void CreateJoker() {
        UnitController.Instance.CreateUnit(0);
    }
    public void CreateSkull() {
        UnitController.Instance.CreateUnit(1);
    }
    public void CreatePanther() {
        UnitController.Instance.CreateUnit(2);
    }
    public void CreateFox() {
        UnitController.Instance.CreateUnit(3);
    }

}
