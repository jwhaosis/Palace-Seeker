using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    private static PlayerController _instance;

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
    }

    // Update is called once per frame
    void Update () {

	}

    public Player GetCurrentPlayer() {
        return playerOne;
    }
}
