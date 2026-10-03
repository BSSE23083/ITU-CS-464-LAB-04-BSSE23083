using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Locks the mouse cursor to the center of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse movement inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate up/down rotation and clamp it so you can't snap your neck backward
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply up/down rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Apply left/right rotation to the player capsule
        playerBody.Rotate(Vector3.up * mouseX);
    }
}