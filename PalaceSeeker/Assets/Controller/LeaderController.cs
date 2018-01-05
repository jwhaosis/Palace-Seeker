using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : MonoBehaviour {

    private static LeaderController _instance;

    public static LeaderController Instance {
        get {
            return _instance;
        }

        set {
            _instance = value;
        }
    }

    private void Awake() {
        _instance = this;
    }

    void Start () {
	}
	
	void Update () {
	}

    public void CreateLeader(int leadCode) {

        Player currentPlayer = PlayerController.Instance.GetCurrentPlayer();

        if (leadCode == 0) {
            currentPlayer.AddLeader(new Hifumi(currentPlayer));
        } else if (leadCode == 1) {
            currentPlayer.AddLeader(new Futaba(currentPlayer));
        }
    }
}
