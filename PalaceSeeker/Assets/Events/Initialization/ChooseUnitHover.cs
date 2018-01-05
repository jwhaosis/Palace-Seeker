using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ChooseUnitHover : MonoBehaviour {

    private static string jokerUnselected = "Units/SelectionSprites/JokerUnselected";
    private static string skullUnselected = "Units/SelectionSprites/SkullUnselected";
    private static string pantherUnselected = "Units/SelectionSprites/PantherUnselected";
    private static string foxUnselected = "Units/SelectionSprites/FoxUnselected";

    private static string jokerSelected = "Units/SelectionSprites/JokerSelected";
    private static string skullSelected = "Units/SelectionSprites/SkullSelected";
    private static string pantherSelected = "Units/SelectionSprites/PantherSelected";
    private static string foxSelected = "Units/SelectionSprites/FoxSelected";

    public void ChangeSpriteUnselect(GameObject button) {
        string buttonSprite = button.name;
        if (buttonSprite.Contains("joker")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(jokerUnselected);
        } else if (buttonSprite.Contains("skull")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(skullUnselected);
        } else if (buttonSprite.Contains("panther")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(pantherUnselected);
        } else if (buttonSprite.Contains("fox")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(foxUnselected);
        }
    }

    public void ChangeSpriteSelect(GameObject button) {
        string buttonSprite = button.name;
        if (buttonSprite.Contains("joker")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(jokerSelected);
        } else if (buttonSprite.Contains("skull")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(skullSelected);
        } else if (buttonSprite.Contains("panther")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(pantherSelected);
        } else if (buttonSprite.Contains("fox")) {
            button.GetComponent<Image>().sprite = Resources.Load<Sprite>(foxSelected);
        }
    }

}
