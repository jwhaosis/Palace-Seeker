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
    private bool menuActive;

    public static ActionsController Instance {
        get {
            return _instance;
        }

        set {
            _instance = value;
        }
    }

    private void Awake() {
        _instance = this;
        actions = new GameObject[2];
    }

    public void CreateActionMenu(Vector3 mouseLocation) {
        if (menuActive == true) {
            return;
        }
        actions[0] = Instantiate(menuOutline);
        //actions[1] = Instantiate(menuName);
        actions[1] = Instantiate(moveButton);
        //actions[3] = Instantiate(attackButton);
        //actions[4] = Instantiate(waitButton);
        //actions[5] = Instantiate(cancelButton);

        //Debug.Log(mouseLocation.x + "," + mouseLocation.y);
        foreach (GameObject buttons in actions) {
            buttons.transform.SetParent(canvas.transform, false);
            float adjustX = canvas.transform.position.x;
            float adjustY = canvas.transform.position.y;
            buttons.transform.position = new Vector3(mouseLocation.x+adjustX, mouseLocation.y+adjustY, 0);
        }
        menuActive = true;

    }

}
