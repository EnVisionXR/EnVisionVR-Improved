using UnityEngine;

public class VibrationController : MonoBehaviour
{
    public Transform virtualObject;
    public float maxDistance = 10f;
    public float maxVibrationIntensity = 1f;

    private OVRInput.Controller controller = OVRInput.Controller.RTouch;
    private OVRHapticsClip hapticsClip;

    private void Start()
    {
        // Create a custom haptics clip
        int clipLength = 20; // Length of the haptics clip (in samples)
        hapticsClip = new OVRHapticsClip(clipLength);
    }

    private void Update()
    {
        // Get the raycast hit position from the right-hand controller
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate the distance ratio between the hit point and the maximum distance
            float distanceRatio = Mathf.Clamp01(hit.distance / maxDistance);

            // Calculate the vibration intensity based on the distance ratio
            float vibrationIntensity = distanceRatio * maxVibrationIntensity;

            // Update the haptics clip based on the vibration intensity
            for (int i = 0; i < hapticsClip.Samples.Length; i++)
            {
                hapticsClip.Samples[i] = (byte)(vibrationIntensity * 255);
            }

            // Play the haptics clip on the right-hand controller
            OVRHaptics.RightChannel.Preempt(hapticsClip);
        }
    }
}
