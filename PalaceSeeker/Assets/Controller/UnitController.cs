using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public GameObject canvas;

    private static UnitController _instance;

    World map;

    Unit selectedUnit;

    Vector3 mouseLocation;

    private void Awake() {
        _instance = this;
        enabled = false;
    }

    void Start () {
    }

    void Update () {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown("space")) {
            CreateMenu();
        }
        SelectUnit();
    }

    //getters and setters--------------------------------------------------
    public static UnitController Instance {
        get {
            return _instance;
        }
    }

    //methods--------------------------------------------------
    public void Initialize(World map) {
        this.map = map;
        enabled = true;
    }

    public void CreateUnit(int charCode) {

        Player currentPlayer = PlayerController.Instance.GetCurrentPlayer();

        for (int y = 0; y<map.Height; y++) {
            for (int x = 0; x < map.Width; x++) {
                if (!Tile.unreachableTypes.Contains(map.GetTile(x, y).Type) && map.GetUnit(x,y)==null) {
                    if (charCode == 0) {
                        map.UnitArray[x, y] = new Joker(this.map, x, y, this);
                    } else if (charCode == 1) {
                        map.UnitArray[x, y] = new Skull(this.map, x, y, this);
                    }
                    else if (charCode == 2) {
                        map.UnitArray[x, y] = new Panther(this.map, x, y, this);
                    }
                    else if (charCode == 3) {
                        map.UnitArray[x, y] = new Fox(this.map, x, y, this);
                    } else {
                        Debug.Log(charCode + " is not a valid character code.");
                    }
                    currentPlayer.AddUnit(map.UnitArray[x, y]);
                    return;
                }
            }
        }
    }

    public void SelectUnit() {

        int x = Mathf.FloorToInt(mouseLocation.x);
        int y = Mathf.FloorToInt(mouseLocation.y);

        if (Input.GetMouseButton(0)) {
            Unit checkSelected = map.GetUnit(x, y);
            if(checkSelected!=selectedUnit && checkSelected!=null) {
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

    public void CreateMenu() {
        Debug.Log("creating menu");
        GameObject moveButton = new GameObject();
        moveButton.transform.SetParent(canvas.transform, true);
        moveButton.AddComponent<SpriteRenderer>();
        moveButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/AttackTile");

        moveButton.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
        moveButton.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));

        float width = moveButton.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float height = moveButton.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float worldScreenHeight = System.Convert.ToSingle(Camera.main.orthographicSize * 2.0);
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        moveButton.GetComponent<SpriteRenderer>().transform.localScale = new Vector3((worldScreenWidth / width)/4, worldScreenHeight / height / 2,0);
    }
}
