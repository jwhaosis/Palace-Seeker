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
        currFramePosition.z = 0;

        CheckKeyboardScroll();
        CheckMouseScroll();

        UpdateZoom();
        UpdateCursor();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;

    }

    private void CheckKeyboardScroll() {
        float translationX = Input.GetAxis("Horizontal");
        float translationY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            Camera.main.transform.Translate(translationX * fastSpeedMultiplier * keyScrollSpeed, translationY * fastSpeedMultiplier * keyScrollSpeed, 0);
        else
            Camera.main.transform.Translate(translationX * keyScrollSpeed, translationY * keyScrollSpeed, 0);
    }
    private void CheckMouseScroll() {

        if (Input.GetMouseButton(2) || Input.GetMouseButton(1)) {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }
    }


    private void UpdateZoom() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 5)
            Camera.main.orthographicSize -= zoomSpeed;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < 15)
            Camera.main.orthographicSize += zoomSpeed;
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