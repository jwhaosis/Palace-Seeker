using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    private static PlayerController _instance;

    public GameObject canvas;

    public GameObject hifumiButtonPF;
    public GameObject futabaButtonPF;

    public GameObject jokerButtonPF;
    public GameObject skullButtonPF;
    public GameObject pantherButtonPF;
    public GameObject foxButtonPF;

    public Player playerOne;
    public Player playerTwo;

    private GameObject[] unitButtons;

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
        _instance = this;

        unitButtons = new GameObject[5];
        this.playerOne = new Player(1);
        this.playerTwo = new Player(2);

        unitButtons[0] = Instantiate(jokerButtonPF);
        unitButtons[1] = Instantiate(skullButtonPF);
        unitButtons[2] = Instantiate(pantherButtonPF);
        unitButtons[3] = Instantiate(foxButtonPF);

        unitButtons[0].transform.SetParent(canvas.transform, false);
        unitButtons[1].transform.SetParent(canvas.transform, false);
        unitButtons[2].transform.SetParent(canvas.transform, false);
        unitButtons[3].transform.SetParent(canvas.transform, false);


        GameObject hifumiButton = Instantiate(hifumiButtonPF);
        GameObject futabaButton = Instantiate(futabaButtonPF);

        hifumiButton.transform.SetParent(canvas.transform, false);
        futabaButton.transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update () {
        if (playerOne.IsComplete()) {
            for(int x = 0; x< unitButtons.Length; x++) {
                GameObject.Destroy(unitButtons[x]);
            }
        }
	}

    public Player GetCurrentPlayer() {
        return playerOne;
    }
}
