using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    private static PlayerController _instance;

    public GameObject canvas;
    public Player playerOne;
    public Player playerTwo;
    public Player currentPlayer;

    public static PlayerController Instance {
        get {
            return _instance;
        }
    }

    // Use this for initialization
    public void Awake () {
        playerOne = new Player(1);
        playerTwo = new Player(2);
        currentPlayer = playerOne;
        _instance = this;
        CreateMenu();
    }

    // Update is called once per frame
    void Update () {
        ClickEndTurn();
    }

    public Player GetCurrentPlayer() {
        return playerOne;
    }


    public void CreateMenu() {
        GameObject endButton = new GameObject { name = "endButton" };
        endButton.transform.SetParent(canvas.transform, true);
        endButton.AddComponent<SpriteRenderer>();
        endButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tiles/EndButton");
        endButton.GetComponent<SpriteRenderer>().transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10));
        endButton.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.7f, 0.8f, 1);
        endButton.AddComponent<BoxCollider2D>();
    }
    public void ClickEndTurn() {
        RaycastHit2D buttonHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetKeyDown(KeyCode.Mouse0) && buttonHit.collider != null && buttonHit.collider.name == "endButton") {
            currentPlayer.RefreshUnits();
            if (currentPlayer == playerOne) {
                Debug.Log("Turn given to player 2");
                currentPlayer = playerTwo;
            } else {
                Debug.Log("Turn given to player 1");
                currentPlayer = playerOne;
            }
            UnitController.Instance.EndTurn();
        }
    }

}
