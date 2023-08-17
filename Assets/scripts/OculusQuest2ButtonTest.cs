using UnityEngine;

public class OculusQuest2ButtonTest : MonoBehaviour
{
    void Update()
    {
        // Check for button presses on the right hand controller
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Debug.Log("Button One (A) pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            Debug.Log("Button Two (B) pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.RTouch))
        {
            Debug.Log("Button Three (X) pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.RTouch))
        {
            Debug.Log("Button Four (Y) pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Primary Index Trigger pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Secondary Index Trigger pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Primary Hand Trigger pressed on the right hand controller");
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Secondary Hand Trigger pressed on the right hand controller");
        }
    }
}
