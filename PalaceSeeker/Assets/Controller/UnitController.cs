using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour {

    private static UnitController _instance;

    World map;

    private Unit selectedUnit;

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

        for(int y = 0; y<map.Height; y++) {
            for (int x = 0; x < map.Width; x++) {
                if (!Tile.unreachableTypes.Contains(map.GetTile(x, y).Type)) {
                    map.UnitArray[x, y] = new Unit(this.map, x, y, this);
                    return;
                }
            }
        }
    }


    private void SelectUnit() {

        int x = Mathf.FloorToInt(mouseLocation.x);
        int y = Mathf.FloorToInt(mouseLocation.y);


        if (Input.GetMouseButton(0)) {
            Unit checkSelected = map.GetUnit(x, y);
            if(checkSelected!=selectedUnit) {
                if (selectedUnit != null) {
                    selectedUnit.Selected = false;
                }
                selectedUnit = checkSelected;
                if (selectedUnit != null) {
                    selectedUnit.Selected = true;
                    Debug.Log("Unit selected at " + x + "," + y + ".");
                }
            }
        } else if (Input.GetMouseButton(1) && selectedUnit != null) {
            selectedUnit.MoveTo(x, y);
            selectedUnit.Selected = false;
            selectedUnit = null;

        }
    }


}
