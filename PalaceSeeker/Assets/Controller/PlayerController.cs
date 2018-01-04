using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    public GameObject canvas;
    public GameObject jokerButtonPF;
    public GameObject skullButtonPF;
    public GameObject pantherButtonPF;
    public GameObject foxButtonPF;

    // Use this for initialization
    void Start () {
        Player playerOne = new Player(1);
        Player playerTwo = new Player(2);
        GameObject jokerButton = Instantiate(jokerButtonPF);
        GameObject skullButton = Instantiate(skullButtonPF);
        GameObject pantherButton = Instantiate(pantherButtonPF);
        GameObject foxButton = Instantiate(foxButtonPF);

        jokerButton.transform.SetParent(canvas.transform, false);
        skullButton.transform.SetParent(canvas.transform, false);
        pantherButton.transform.SetParent(canvas.transform, false);
        foxButton.transform.SetParent(canvas.transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
