using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public enum ActionButtons { Attack, SpecialOne, SpecialTwo, SpecialThree, Unclicked }

    public GameObject canvas;

    private static UnitController _instance;

    World map;
    Unit selectedUnit;
    bool menuActive;
    ActionButtons actionButton;

    Vector3 mouseLocation;

    private void Awake() {
        actionButton = ActionButtons.Unclicked;
        _instance = this;
        enabled = false;
    }

    void Start () {
    }

    void Update () {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseLocation.x += 0.5f;
        mouseLocation.y += 0.5f;
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

        RandomSpawns();
    }

    private void RandomSpawns() {
        int x, y, min;
        min = 6;
        for (int a = 0; a < 2; a++) {
            do {
                x = Random.Range(min, 20);
                y = Random.Range(min, 20);
            } while (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type));
            map.UnitArray[x, y] = new PyroJack(this.map, x, y);
            do {
                x = Random.Range(0, min);
                y = Random.Range(min, 20);
            } while (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type));
            map.UnitArray[x, y] = new PyroJack(this.map, x, y);
            do {
                x = Random.Range(min, 20);
                y = Random.Range(0, min);
            } while (Tile.unreachableTypes.Contains(map.GetTile(x, y).Type));
            map.UnitArray[x, y] = new PyroJack(this.map, x, y);
            min += 10;
        }
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

        if (selectedUnit!= null && selectedUnit.Moved) {
            RaycastHit2D buttonHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(buttonHit.collider == null) {
                if(actionButton.Equals(ActionButtons.Unclicked)) {
                    selectedUnit.ClearAllGrids();
                }
                return;
            } else if (buttonHit.collider.name == "attackButton") {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    Debug.Log("Attack Button Clicked.");
                    actionButton = ActionButtons.Attack;
                } else if (actionButton != ActionButtons.Attack && buttonHit.collider.name.Contains("Button")) {
                    actionButton = ActionButtons.Unclicked;
                }
                selectedUnit.GenerateGrid(selectedUnit.RangeMax, Unit.UnitCommands.Attack);
            } else if (buttonHit.collider.name == "specialButton") {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    Debug.Log("Special Button Clicked.");
                    actionButton = ActionButtons.SpecialOne;
                } else if (actionButton != ActionButtons.SpecialOne && buttonHit.collider.name.Contains("Button")) {
                    actionButton = ActionButtons.Unclicked;
                }
                selectedUnit.SpecialOneGrid();
            }
        }
    }

    public void EndTurn() {
        this.selectedUnit = null;
        this.actionButton = ActionButtons.Unclicked;
        this.menuActive = false;
    }
}
