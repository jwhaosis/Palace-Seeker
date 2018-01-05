using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    private static PlayerController _instance;

    public Player playerOne;
    public Player playerTwo;

    public static PlayerController Instance {
        get {
            return _instance;
        }

        set {
            _instance = value;
        }
    }

    // Use this for initialization
    void Start () {

        this.playerOne = new Player(1);
        this.playerTwo = new Player(2);
        _instance = this;
    }

    // Update is called once per frame
    void Update () {

	}

    public Player GetCurrentPlayer() {
        return playerOne;
    }
}
