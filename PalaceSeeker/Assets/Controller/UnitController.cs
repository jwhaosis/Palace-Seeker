using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    private static UnitController _instance;

    World map;

    public static UnitController Instance {
        get {
            return _instance;
        }
    }

    public Sprite foxSprite;

    private Unit selectedUnit;
    private GameObject selectedObject;

    private Vector3 mouseLocation;

    // Use this for initialization
    void Start () {
        _instance = this;
    }

    // Update is called once per frame
    void Update () {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(mouseLocation.x);
        int y = Mathf.FloorToInt(mouseLocation.y);

        SelectUnit();
        MoveUnit();
    }

    public void CreateUnit(World map) {
        this.map = map;
        this.map.UnitArray[0,0] = new Unit(this.map, 0, 0);

        GameObject foxObject = new GameObject {
            name = "Fox"
        };
        foxObject.AddComponent<SpriteRenderer>();
        foxObject.transform.position = new Vector3(0, 0, 0);
        foxObject.transform.SetParent(this.transform, true);
        foxObject.GetComponent<SpriteRenderer>().sprite = foxSprite;
        selectedObject = foxObject;
    }

    private void SelectUnit() {

        int x = Mathf.FloorToInt(mouseLocation.x);
        int y = Mathf.FloorToInt(mouseLocation.y);

        if (Input.GetMouseButton(0)) {
            if (selectedUnit != null || map.UnitArray[x,y]==null) {
                return;
            }
            //selectedObject.GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log("Unit selected at " + x + "," + y + ".");
            selectedUnit = map.UnitArray[x, y];
        }
        if (Input.GetMouseButton(2)) {
            if (selectedUnit == null) {
                return;
            }
            //selectedObject.GetComponent<SpriteRenderer>().color = original;
            Debug.Log("Unit deselected.");
            selectedUnit = null;
        }

    }

    private void MoveUnit() {

        int x = Mathf.FloorToInt(mouseLocation.x);
        int y = Mathf.FloorToInt(mouseLocation.y);

        if (Input.GetMouseButton(1)) {
            if (selectedUnit == null || !selectedUnit.MoveTo(x, y)) {
                selectedUnit = null;
                return;
            }
            Debug.Log("Unit moved to " + x + "," + y + ".");
            selectedObject.transform.position = new Vector3(x, y, 0);
            //selectedObject.GetComponent<SpriteRenderer>().color = original;
            selectedUnit = null;
        }


    }
}
