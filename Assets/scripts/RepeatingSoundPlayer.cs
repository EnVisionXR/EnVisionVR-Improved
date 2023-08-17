using System.Media;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Security.AccessControl;

public class RepeatingSoundPlayer : MonoBehaviour
{
    private Transform virtualObjectTransform;
    public Transform leftcontroller;
    public Transform rightcontroller;
    public Transform controller;
    public string hand = "right";
    public float maxDistance = 10f;
    public float maxInterval = 3f;
    private AudioSource audioSource;
    private float timer = 0f; // Timer to track the elapsed time
    private bool isTriggered = false;
    public InputHelpers.Button secondaryButton;
    public bool secondaryButtonDown;
    public XRController rightHand;
    private CameraFieldOfView camerafieldofview;
    private float directionTimer = 0f;

    private void Start()
    {
        // Check if an AudioSource component is already attached
        if (audioSource == null)
        {
            // Create and attach a new AudioSource component
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Load the audio clip from the Resources folder
        AudioClip clip = Resources.Load<AudioClip>("beep");

        // Set the audio clip for the repeating sound
        audioSource.clip = clip;
        audioSource.volume = 0.35f;

        camerafieldofview = GameObject.Find("CameraController").GetComponent<CameraFieldOfView>();
    }

    //public void UpdatePosition(Transform virtualObject)
    public void Update()
    {
        secondaryButtonDown = false;
        rightHand.inputDevice.IsPressed(secondaryButton, out secondaryButtonDown);
        if (isTriggered)
        {
            timer += Time.deltaTime;
            directionTimer += Time.deltaTime;

            if (hand == "left")
                controller = leftcontroller;
            else
            {
                controller = rightcontroller;
            }

            // Calculate the direction from the controller
            Vector3 controllerDirection = controller.forward;

            RaycastHit hit;
            bool hasHit = Physics.Raycast(controller.position, controllerDirection, out hit, maxDistance);
            //Debug.Log("Hit.Transform:" + hit.transform);
            //.Log("Virtual Object Transform:" + virtualObjectTransform);
            //if (hasHit && hit.transform == virtualObjectTransform)
            //{
            float distanceToObject = (controller.position - virtualObjectTransform.position).magnitude;
            // Natural Language Instructions to Find Virtual Object
            if (directionTimer >= 4f)
            {
                // Reset the timer
                directionTimer = 0f;
                
                float deltaX = virtualObjectTransform.position.x - controller.position.x;
                float deltaY = virtualObjectTransform.position.y - controller.position.y;
                float deltaZ = virtualObjectTransform.position.z - controller.position.z;

                // Get the main camera's rotation quaternion
                Quaternion cameraRotation = camerafieldofview.mainCamera.transform.rotation;

                // Convert the quaternion to a rotation matrix
                Matrix4x4 rotationMatrix = Matrix4x4.Rotate(cameraRotation);

                // Extract the right, up, and forward vectors from the rotation matrix
                Vector3 cameraRight = rotationMatrix.GetColumn(0);
                Vector3 cameraUp = rotationMatrix.GetColumn(1);
                Vector3 cameraForward = rotationMatrix.GetColumn(2);

                // Calculate the dot products of your vector with each of these extracted vectors
                Vector3 targetDirection = (virtualObjectTransform.position - controller.position).normalized;

                float dotRight = Vector3.Dot(targetDirection, cameraRight);
                float dotUp = Vector3.Dot(targetDirection, cameraUp);
                float dotForward = Vector3.Dot(targetDirection, cameraForward);

                // Determine the direction based on dot product comparisons
                if (Mathf.Abs(dotForward) >= Mathf.Abs(dotRight) && Mathf.Abs(dotForward) >= Mathf.Abs(dotUp))
                {
                    if (dotForward > 0)
                        camerafieldofview.SpeakText("forward");
                    else
                        camerafieldofview.SpeakText("backward");
                }
                else if (Mathf.Abs(dotRight) >= Mathf.Abs(dotForward) && Mathf.Abs(dotRight) >= Mathf.Abs(dotUp))
                {
                    if (dotRight > 0)
                        camerafieldofview.SpeakText("right");
                    else
                        camerafieldofview.SpeakText("left");
                }
                else
                {
                    if (dotUp > 0)
                        camerafieldofview.SpeakText("upward");
                    else
                        camerafieldofview.SpeakText("downward");
                }
            }
        

            //float normalizedDistance = hit.distance / maxDistance;
            float normalizedDistance = distanceToObject / maxDistance;
            float timeInterval = Mathf.Lerp(0.2f, maxInterval, normalizedDistance);

            // Check if the timer has reached the interval
            if (timer >= timeInterval)
            {
                // Reset the timer
                timer = 0f;

                // Play the sound
                audioSource.Play();
            }

            if (distanceToObject< 0.15f)
            {
                rightHand.SendHapticImpulse(0.5f, 0.5f);
            }

            //}
            if (secondaryButtonDown)
            {
                Debug.Log("Secondary button pressed!");
                camerafieldofview.localizationMode = false;
                DeactivateBeep();
            }
        }
        else
        {

        }
    }

    public void TriggerBeep(Transform ObjectToLocateTransform, string handinput)
    {
        hand = handinput;
        if (ObjectToLocateTransform == null)
        {
            Debug.LogError("Beeping Virtual Object not assigned!");
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            isTriggered = true;
            virtualObjectTransform = ObjectToLocateTransform;
        }
    }

    public void DeactivateBeep()
    {
        isTriggered= false;
    }

}
