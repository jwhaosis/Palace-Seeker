using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseUnit : MonoBehaviour {

    public void ChooseHifumi() {
        LeaderController.Instance.CreateLeader(0);
    }
    public void ChooseFutaba() {
        LeaderController.Instance.CreateLeader(1);
    }

    public void CreateJoker() {
        UnitController.Instance.CreateUnit(0);
    }
    public void CreateSkull() {
        UnitController.Instance.CreateUnit(1);
    }
    public void CreatePanther() {
        UnitController.Instance.CreateUnit(2);
    }
    public void CreateFox() {
        UnitController.Instance.CreateUnit(3);
    }

    public void DisableButton(GameObject button) {
        button.SetActive(false);
    }

    public void ChangeSprite(GameObject button) {
        button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Units/SelectionSprites/JokerSelected");
    }
}
