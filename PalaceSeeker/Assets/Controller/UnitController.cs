using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public enum ActionButtons { Attack, SpecialOne, SpecialTwo, SpecialThree }

    public GameObject canvas;

    private static UnitController _instance;

    World map;
    Unit selectedUnit;
    bool menuActive;
    ActionButtons actionButton;

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
        ClickMenuButton();
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
        map.TileArray[4, 4].Type = Tile.TileType.Floor;
        map.UnitArray[4, 4] = new PyroJack(this.map, 4, 4);

        map.TileArray[6, 2].Type = Tile.TileType.Floor;
        map.UnitArray[6, 2] = new PyroJack(this.map, 6, 2);

    }

    public void CreateUnit(int charCode) {

        Player currentPlayer = PlayerController.Instance.GetCurrentPlayer();

        for (int y = 0; y<map.Height; y++) {
            for (int x = 0; x < map.Width; x++) {
                if (!Tile.unreachableTypes.Contains(map.GetTile(x, y).Type) && map.GetUnit(x,y)==null) {
                    if (charCode == 0) {
                        map.UnitArray[x, y] = new Joker(this.map, x, y);
                    } else if (charCode == 1) {
                        map.UnitArray[x, y] = new Skull(this.map, x, y);
                    }
                    else if (charCode == 2) {
                        map.UnitArray[x, y] = new Panther(this.map, x, y);
                    }
                    else if (charCode == 3) {
                        map.UnitArray[x, y] = new Fox(this.map, x, y);
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

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Unit checkSelected = map.GetUnit(x, y);
            if(checkSelected != null && (checkSelected.unitType == "enemy" || checkSelected.Controller!=PlayerController.Instance.GetCurrentPlayer())) {
                return;
            }
            if (checkSelected != null && checkSelected != selectedUnit && !checkSelected.TurnFinished) {
                if (selectedUnit != null) {
                    if (selectedUnit.Moved) {
                        Debug.Log("Finish current unit's turn.");
                        return;
                    }
                    selectedUnit.Selected = false;
                }
                selectedUnit = checkSelected;
                selectedUnit.Selected = true;
                Debug.Log("Unit selected at " + x + "," + y + ".");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && selectedUnit != null && !selectedUnit.Moved && selectedUnit.MoveTo(x, y)) {
            if (!this.menuActive) {
                CreateMenu();
                this.menuActive = true;
                selectedUnit.Moved = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && selectedUnit != null && selectedUnit.Moved) {
            if (this.menuActive && actionButton==ActionButtons.Attack && selectedUnit.AttackSquare(x, y)) {
                selectedUnit.Selected = false;
                selectedUnit.Moved = false;
                selectedUnit.TurnFinished = true;
                selectedUnit = null;
                this.menuActive = false;
            } else if (this.menuActive && actionButton == ActionButtons.SpecialOne && selectedUnit.SpecialOne(x, y)) {
                selectedUnit.Selected = false;
                selectedUnit.Moved = false;
                selectedUnit.TurnFinished = true;
                selectedUnit = null;
                this.menuActive = false;
            }
        }
    }


    //delete these and move them to a new monobehavior probably
    public void CreateUnitFrames() {
        GameObject unitFrame1 = new GameObject();
        unitFrame1.transform.SetParent(canvas.transform, true);
        unitFrame1.AddComponent<SpriteRenderer>();
        unitFrame1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/UnitMenu");
        GameObject unitFrame2 = new GameObject();
        unitFrame2.transform.SetParent(canvas.transform, true);
        unitFrame2.AddComponent<SpriteRenderer>();
        unitFrame2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/UnitMenu");

        float width = unitFrame1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float height = unitFrame1.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        float worldScreenHeight = System.Convert.ToSingle(Camera.main.orthographicSize * 2.0);
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        unitFrame1.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
        unitFrame1.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        unitFrame2.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
        unitFrame2.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, worldScreenHeight / height / 1.185f, 10));

        unitFrame1.GetComponent<SpriteRenderer>().transform.localScale = new Vector3((worldScreenWidth / width)/ 4.555f, worldScreenHeight / height / 2,0);
        unitFrame2.GetComponent<SpriteRenderer>().transform.localScale = new Vector3((worldScreenWidth / width) / 4.555f, worldScreenHeight / height / 2, 0);
    }

    public void CreateMenu() {
        GameObject attackButton = new GameObject { name = "attackButton" };
        attackButton.transform.SetParent(canvas.transform, true);
        attackButton.AddComponent<SpriteRenderer>();
        attackButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/AttackButton");
        attackButton.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 10));
        attackButton.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.7f, 0.8f, 1);
        attackButton.AddComponent<BoxCollider2D>();

        GameObject specialButton = new GameObject {name = "specialButton"};
        specialButton.transform.SetParent(canvas.transform, true);
        specialButton.AddComponent<SpriteRenderer>();
        specialButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/SpecialButton");
        specialButton.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));
        specialButton.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.7f, 0.8f, 1);
        specialButton.AddComponent<BoxCollider2D>();
    }

    public void ClickMenuButton() {

        if (Input.GetKeyDown(KeyCode.Mouse0) && selectedUnit!= null) {
            RaycastHit2D buttonHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(buttonHit.collider == null) {
                return;
            } else if (buttonHit.collider.name == "attackButton") {
                Debug.Log("Attack Button Clicked.");
                selectedUnit.GenerateAttackGrid();
                actionButton = ActionButtons.Attack;
            } else if (buttonHit.collider.name == "specialButton") {
                Debug.Log("Special Button Clicked.");
                selectedUnit.SpecialOneGrid();
                actionButton = ActionButtons.SpecialOne;
            }
        }
    }
}
