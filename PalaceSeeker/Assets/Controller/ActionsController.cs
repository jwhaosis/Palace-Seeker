using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsController : MonoBehaviour {

    private static ActionsController _instance;

    public GameObject canvas;

    public GameObject menuName;
    public GameObject menuOutline;

    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject waitButton;
    public GameObject cancelButton;

    private GameObject[] actions;
    public Unit selectedUnit;

    public static ActionsController Instance {
        get {
            return _instance;
        }
    }

    void Start() {
        _instance = this;
        actions = new GameObject[2];
    }

    public void CreateActionMenu(Unit selectedUnit, Vector3 mouseLocation) {
        this.selectedUnit = selectedUnit;

        actions[0] = Instantiate(menuOutline);
        //actions[1] = Instantiate(menuName);
        actions[1] = Instantiate(moveButton);
        //actions[3] = Instantiate(attackButton);
        //actions[4] = Instantiate(waitButton);
        //actions[5] = Instantiate(cancelButton);

        //Debug.Log(mouseLocation.x + "," + mouseLocation.y);
        foreach (GameObject buttons in actions) {
            buttons.transform.SetParent(canvas.transform, false);
        }
    }

    public void moveUnit() {
        Instantiate(menuName).transform.SetParent(canvas.transform, false);
        selectedUnit.Selected = true;
    }

}
