using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    private static UnitController _instance;

    World map;

    private Unit selectedUnit;

    public Sprite foxSprite;
    private Vector3 mouseLocation;

    // Use this for initialization
    void Start () {
        _instance = this;
        enabled = false;
    }

    // Update is called once per frame
    void Update () {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SelectUnit();
        MoveUnit();
    }

    //getters and setters--------------------------------------------------
    public static UnitController Instance {
        get {
            return _instance;
        }
    }

    //methods--------------------------------------------------
    public void CreateUnit(World map) {
        enabled = true;
        this.map = map;

        GameObject unitObject = new GameObject {
            name = "Fox"
        };
        unitObject.AddComponent<SpriteRenderer>();
        unitObject.transform.position = new Vector3(0, 0, 0);
        unitObject.transform.SetParent(this.transform, true);
        unitObject.GetComponent<SpriteRenderer>().sprite = foxSprite;

        this.map.UnitArray[0,0] = new Unit(this.map, 0, 0, unitObject);
    }


    private void SelectUnit() {

        if (Input.GetMouseButton(0)) {
            int x = Mathf.FloorToInt(mouseLocation.x);
            int y = Mathf.FloorToInt(mouseLocation.y);

            Unit checkSelected = map.GetUnit(x, y);
            if(checkSelected!=selectedUnit) {
                selectedUnit = checkSelected;
                if (selectedUnit != null) {
                    Debug.Log("Unit selected at " + x + "," + y + ".");
                }
            }
        }
    }


    private void MoveUnit() {

        if (Input.GetMouseButton(1) && selectedUnit!= null) {
            int x = Mathf.FloorToInt(mouseLocation.x);
            int y = Mathf.FloorToInt(mouseLocation.y);

            selectedUnit.MoveTo(x,y);
            selectedUnit = null;
        }
    }
}
