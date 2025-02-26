using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCamera : MonoBehaviour
{
    private Camera myCamera; // Reference to the camera component

    void Start()
    {
        // Get the Camera component attached to this GameObject
        myCamera = this.GetComponent<Camera>();

        // Optionally, start with the camera disabled
        if (myCamera != null)
        {
            myCamera.enabled = false;
        }
    }

    void Update()
    {
        // Check if the left mouse button was pressed down this frame
        if (Input.GetMouseButtonDown(0))
        {
            // Enable the camera component
            if (myCamera != null)
            {
                myCamera.enabled = true;
            }
        }

        // Check if the left mouse button was released this frame
        if (Input.GetMouseButtonUp(0))
        {
            // Disable the camera component
            if (myCamera != null)
            {
                myCamera.enabled = false;
            }
        }
    }
}
