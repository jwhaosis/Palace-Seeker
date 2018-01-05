using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationScript : MonoBehaviour {

    private GameObject[] unitButtons;
    private GameObject[] leaderButtons;

    public GameObject canvas;

    public GameObject hifumiButtonPF;
    public GameObject futabaButtonPF;

    public GameObject jokerButtonPF;
    public GameObject skullButtonPF;
    public GameObject pantherButtonPF;
    public GameObject foxButtonPF;

    // Use this for initialization
    void Start () {
        unitButtons = new GameObject[4];
        leaderButtons = new GameObject[2];

        unitButtons[0] = Instantiate(jokerButtonPF);
        unitButtons[1] = Instantiate(skullButtonPF);
        unitButtons[2] = Instantiate(pantherButtonPF);
        unitButtons[3] = Instantiate(foxButtonPF);

        unitButtons[0].transform.SetParent(canvas.transform, false);
        unitButtons[1].transform.SetParent(canvas.transform, false);
        unitButtons[2].transform.SetParent(canvas.transform, false);
        unitButtons[3].transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update () {
        if (PlayerController.Instance.playerOne.IsCompleteUnits() && leaderButtons[0] == null) {

            foreach (GameObject button in unitButtons) {
                Destroy(button);
            }

            leaderButtons[0] = Instantiate(hifumiButtonPF);
            leaderButtons[1] = Instantiate(futabaButtonPF);

            leaderButtons[0].transform.SetParent(canvas.transform, false);
            leaderButtons[1].transform.SetParent(canvas.transform, false);
        }

        if (PlayerController.Instance.playerOne.IsCompleteAll()) {
            foreach (GameObject button in leaderButtons) {
                Destroy(button);
            }
            enabled = false;
        }
    }
}
