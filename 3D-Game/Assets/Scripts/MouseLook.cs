using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject gameOverCanvas; // Reference to the Game Over canvas
    public GameObject winCanvas; // Reference to the Win canvas

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the cursor is locked at the start
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused or GameOver canvas is active or Win canvas is active
        if (Time.timeScale == 0f || gameOverCanvas.activeSelf || winCanvas.activeSelf)
        {
            // Unlock the cursor to show it on screen
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; // Exit the method, don't proceed with mouse movement
        }
        else
        {
            // Ensure the cursor is locked and hidden when the game is not paused or GameOver canvas is not active
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
