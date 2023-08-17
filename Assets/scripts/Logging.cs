using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using System;


public class Logging : MonoBehaviour
{
    Camera mainCamera;
    public XRController rightHand;
    public XRController leftHand;
    public InputHelpers.Button primaryButton;
    public InputHelpers.Button secondaryButton;
    public InputHelpers.Button triggerButton;
    public bool leftprimaryButtonDown;
    public bool leftsecondaryButtonDown;
    public bool lefttriggerButtonDown;
    public bool primaryButtonDown;
    public bool secondaryButtonDown;
    public bool triggerButtonDown;

    void Start()
    {
        mainCamera = Camera.main;
    }

    async void Update()
    {
        // Check if the button on the right hand controller is pressed
        primaryButtonDown = false;//OVRInput.Get(OVRInput.Button.One);
        rightHand.inputDevice.IsPressed(primaryButton, out primaryButtonDown);
        if (primaryButtonDown)
        {
            Debug.LogError("Right primary button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        secondaryButtonDown = false;
        rightHand.inputDevice.IsPressed(secondaryButton, out secondaryButtonDown);
        if (secondaryButtonDown)
        {
            Debug.LogError("Right secondary button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        triggerButtonDown = false;
        rightHand.inputDevice.IsPressed(triggerButton, out triggerButtonDown);
        if (triggerButtonDown)
        {
            Debug.LogError("Right trigger button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        leftprimaryButtonDown = false;
        leftHand.inputDevice.IsPressed(primaryButton, out leftprimaryButtonDown);
        if (leftprimaryButtonDown)
        {
            Debug.LogError("Left primary button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        leftsecondaryButtonDown = false;
        leftHand.inputDevice.IsPressed(secondaryButton, out leftsecondaryButtonDown);
        if (leftsecondaryButtonDown)
        {
            Debug.LogError("Left secondary button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        lefttriggerButtonDown = false;
        leftHand.inputDevice.IsPressed(triggerButton, out lefttriggerButtonDown);
        if (lefttriggerButtonDown)
        {
            Debug.LogError("Left trigger button pressed!");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("q"))
        {
            Debug.LogError("Beginning of Task 1a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("w"))
        {
            Debug.LogError("End of Task 1a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("e"))
        {
            Debug.LogError("Beginning of Task 1b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("r"))
        {
            Debug.LogError("End of Task 1b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("t"))
        {
            Debug.LogError("Beginning of Task 1c)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("y"))
        {
            Debug.LogError("End of Task 1c)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("a"))
        {
            Debug.LogError("Beginning of Task 2a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("s"))
        {
            Debug.LogError("End of Task 2a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("d"))
        {
            Debug.LogError("Beginning of Task 2b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("f"))
        {
            Debug.LogError("End of Task 2b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("z"))
        {
            Debug.LogError("Beginning of Task 3a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("x"))
        {
            Debug.LogError("End of Task 3a)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("c"))
        {
            Debug.LogError("Beginning of Task 3b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }
        if (Input.GetKeyDown("v"))
        {
            Debug.LogError("End of Task 3b)");
            Debug.LogError("Camera Position:" + mainCamera.transform.position);
            Debug.LogError("Camera Orientation:" + mainCamera.transform.rotation);
            Debug.LogError("Left Controller Position:" + leftHand.transform.position);
            Debug.LogError("Right Controller Position:" + rightHand.transform.position);
            Debug.LogError("Current Time:" + Time.time);
        }

    }
}
