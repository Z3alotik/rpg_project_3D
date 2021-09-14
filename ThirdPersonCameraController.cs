using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    //turning speed of camera
    public float lookSensitivity = 1;
    public Transform target;
    public Transform player;

    //input values from mouse
    float mouseX;
    float mouseY;

    void Start()
    {
        //hiding the cursor during runtime
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        ControlCamera();
    }

    void ControlCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * lookSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * lookSensitivity;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(target);

        //rotate the player
        player.rotation = Quaternion.Euler(0, mouseX, 0);
        //rotate the camera
        target.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }
}
