using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
    public float fastSpeedMultiplier = 2;
    public float keyScrollSpeed = 2;

    public int zoomSpeed = 1;

    private Vector3 currFramePosition;
    private Vector3 lastFramePosition;
    public GameObject circleCursor;

    void Start() {

    }

    void Update() {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.x += 0.5f;
        currFramePosition.y += 0.5f;
        currFramePosition.z = 0;

        UpdateCursor();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;

    }

    private void UpdateCursor() {
        Tile tileUnderMouse = WorldController.Instance.GetTileAtCoordinate(currFramePosition);
        if(tileUnderMouse != null) {
            circleCursor.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            circleCursor.transform.position = cursorPosition;
        }
        else {
            circleCursor.SetActive(false);
        }
    }
    
}