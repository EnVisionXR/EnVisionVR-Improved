using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using System.Xml;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices.ComTypes;

public class RaycastReadObject : MonoBehaviour
{
    CameraFieldOfView cameraFieldOfView;
    CameraController cameraController;
    ButtonCheckFieldOfView buttonCheckFieldOfView;
    bool IsCheckFoV = false;
    private bool prevPrimaryButtonState_ = false;
    private bool prevTriggerButtonState_ = false;
    private bool rayReadTrigger = false;
    private float timer = 0f; // Timer to track the elapsed time
    public Transform controller;
    public float maxDistance = 10f;
    public float maxInterval = 3f;
    private AudioSource audioSource;
    public List<GameObject> readObjects;
    public float newImportance;
    public float originalImportance;
    public Dictionary<string, float> newImportanceValues;
    int i = 0;
    int j = 0;
    private bool isSpoken = false;

    //public bool highlight = false;
    //private bool priorHighlight = false;
    //public bool Guideline = false;
    //private bool priorGuidline = false;
    //public bool dynamicScanning = false;
    //public Color highlightColor = Color.green;
    //private Color priorHighlighColor = Color.white;
    //public Color guidelineColor = Color.red;
    //private Color priorGuidlineColor = Color.white;
    //public float forwardFactor = 0.5f;
    //private float priorForwardFactor = 0.5f;
    //public float radius = 0.25f;
    //private float priorRadius = 0.25f;

    void Start()
    {
        cameraFieldOfView = GetComponent<CameraFieldOfView>();
        cameraController = GetComponent<CameraController>();
        buttonCheckFieldOfView= GetComponent<ButtonCheckFieldOfView>(); 
        readObjects = new List<GameObject>();
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

        //GameObject[] allObjects = FindObjectsOfType<GameObject>();
        //foreach (GameObject obj in allObjects)
        //{
        //    if (obj.activeInHierarchy && obj.isSalience())
        //    {
        //        AddContours contours = obj.AddComponent<AddContours>();
        //    }
        //}
    }

    private async void Update()
    {
        if (cameraFieldOfView.leftprimaryButtonDown && !prevPrimaryButtonState_)
        {
            if (buttonCheckFieldOfView.beginnerMode)
            {
                cameraFieldOfView.SpeakText("Left primary button pressed!");
                await Task.Delay((int)(0));
                cameraFieldOfView.SpeakText("Main Object function activated!");
            }
            //await Task.Delay((int)(0));
            TriggerCheckFoV();
            //ReadObjectInFieldOfView(readObjects);
        }
        OnCheckFieldOfViewButtonClick();

        if (cameraFieldOfView.lefttriggerButtonDown && !prevTriggerButtonState_ && !isSpoken)
        {
            if (buttonCheckFieldOfView.beginnerMode)
            {
                cameraFieldOfView.SpeakText("Left trigger button pressed!");
                cameraFieldOfView.SpeakText("Raycast distance function activated!");
                isSpoken = true;
            }
        }

        if (!cameraFieldOfView.lefttriggerButtonDown && prevTriggerButtonState_ && isSpoken)
        {
            isSpoken = false;
        }

        //await Task.Delay((int)(0));
        if (cameraFieldOfView.lefttriggerButtonDown)
        {
            timer += Time.deltaTime;

            // Calculate the direction from the controller
            Vector3 controllerDirection = controller.forward;

            RaycastHit hit;
            bool hasHit = Physics.Raycast(controller.position, controllerDirection, out hit, maxDistance);

            float normalizedDistance = hit.distance / maxDistance;
            float timeInterval = Mathf.Lerp(0.2f, maxInterval, normalizedDistance);

            // Check if the timer has reached the interval
            if (timer >= timeInterval)
            {
                // Reset the timer
                timer = 0f;

                // Play the sound
                audioSource.Play();
            }

        }
        prevTriggerButtonState_ = cameraFieldOfView.triggerButtonDown;
        prevPrimaryButtonState_ = cameraFieldOfView.leftprimaryButtonDown;
    }
    //public async void ReadObjectInFieldOfView(List<GameObject> readObjects)
    //{
    //    if (cameraFieldOfView.mainCamera == null)
    //    {
    //        Debug.LogError("Main camera is not assigned!");
    //        return;
    //    }

    //    GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();

    //    List<GameObject> objectsInFieldOfView = new List<GameObject>();

    //    foreach (GameObject obj in objectsInScene)
    //    {
    //        // Get the object's transform component to retrieve its position
    //        Transform objTransform = obj.transform;
    //        Vector3 objPosition = objTransform.position;
    //        //Debug.LogError("Checking visibility of: " + obj.name);
    //        // Check if the object is within the camera's field of view
    //        if (cameraFieldOfView.IsObjectVisible(objPosition, cameraFieldOfView.mainCamera.transform.position, cameraFieldOfView.mainCamera.transform.rotation, cameraFieldOfView.mainCamera.fieldOfView))
    //        {
    //            objectsInFieldOfView.Add(obj);
    //        }
    //    }

    //    newImportanceValues = new Dictionary<string, float>(cameraFieldOfView.importanceValues);
    //    //foreach (KeyValuePair<string, float> entry in newImportanceValues)
    //    //{
    //    //    Debug.Log("NewImportance Dict:" + entry.Key + entry.Value);
    //    //}

    //    // Calculate the new importance values for each object based on the distance from the camera
    //    foreach (GameObject obj in objectsInFieldOfView)
    //    {
    //        Debug.Log(obj.name + " In FoV");
    //        float originalImportance = cameraFieldOfView.GetImportanceValue(obj.name);
    //        if (originalImportance != 0)
    //            Debug.Log("Line 144: " + originalImportance);
    //        float distance = Vector3.Distance(obj.transform.position, cameraFieldOfView.mainCamera.transform.position);
    //        newImportance = originalImportance * (Mathf.Exp(-distance) - Mathf.Exp(-5));
    //        if (newImportance != 0)
    //            Debug.Log("Line 147:" + newImportance);
    //        int objectDecay = readObjects.Where(x => x.Equals(obj)).Count();
    //        newImportance = (float)(newImportance * Math.Pow(0.5, objectDecay));
    //        if (newImportance != 0)
    //            Debug.Log("Line 150:" + newImportance);
    //        if (distance > 5f)
    //            newImportance = 0;
    //        newImportanceValues[obj.name] = newImportance; // Update the importance value in the dictionary
    //        //cameraFieldOfView.importanceValues[obj.name] = newImportance;
    //        if (newImportance!=0)
    //            Debug.Log("In FoV:" + obj.name + "Original Importance" + originalImportance + "New" + newImportance);
    //    }

    //    // Sort the objects based on the new importance values
    //    objectsInFieldOfView.Sort((a, b) => newImportanceValues[b.name].CompareTo(newImportanceValues[a.name]));
    //    //objectsInFieldOfView.Sort((a, b) => cameraFieldOfView.GetImportanceValue(b.name).CompareTo(cameraFieldOfView.GetImportanceValue(a.name)));

    //    foreach (GameObject obj in objectsInFieldOfView)
    //    {
    //        i = i + 1;
    //        if (i < 5)
    //        {
    //            Debug.Log("FoV Sorted:" + obj.name);
    //            Debug.Log("Original Importance:" + cameraFieldOfView.GetImportanceValue(obj.name));
    //            Debug.Log("New Importance:" + newImportanceValues[obj.name]);
    //        }
    //    }

    //    //foreach (GameObject obj in readObjects)
    //    //{
    //    //    j = j + 1;
    //    //    if (j<5)
    //    //        Debug.Log("Read Objects:" + obj.name);
    //    //}

    //    // Print the object with the highest new importance values
    //    for (int i = 0; i < Mathf.Min(1, objectsInFieldOfView.Count); i++)
    //    {
    //        GameObject obj = objectsInFieldOfView[i];
    //        if (cameraFieldOfView.GetImportanceValue(obj.name) > 0 && cameraFieldOfView.localizationMode == false)
    //        {
    //            Debug.Log(obj.name + ": " + newImportance);
    //            string message = obj.name;
    //            string messageFull = obj.name + " is within the field of view. Position: " + obj.transform.position + " Importance: " + cameraFieldOfView.GetImportanceValue(obj.name);
    //            Debug.LogError(messageFull);
    //            cameraFieldOfView.SpeakText(message.Replace("_", " "));
    //            await Task.Delay((int)(0));
    //            readObjects.Add(obj);

    //            // Store the most recently spoken object
    //            cameraFieldOfView.recentlySpokenObject = obj;

    //            // Delay before playing the audio clip
    //            //float delay = 2.0f; // Adjust the delay time as needed
    //            await Task.Delay((int)(delay * 8000));

    //            // Play the audio clip at the object's position
    //            string path = obj.name;
    //            Transform currentTransform = obj.transform;

    //            while (currentTransform.parent != null)
    //            {
    //                currentTransform = currentTransform.parent;
    //                path = currentTransform.name + "/" + path;
    //            }

    //            //Debug.LogError("Position for" + obj.name + ": " + obj.transform.position);
    //            if (currentTransform.name == "Interactables")
    //            {
    //                cameraFieldOfView.soundController.PlayAudioClip("Positive Notification", obj.transform.position);
    //            }
    //            else if (currentTransform.name == "Interior")
    //            {
    //                cameraFieldOfView.soundController.PlayAudioClip("Magic Spell", obj.transform.position);
    //            }
    //            else if (currentTransform.name == "Potions")
    //            {
    //                cameraFieldOfView.soundController.PlayAudioClip("Magic", obj.transform.position);
    //            }
    //            else if (currentTransform.name == "Exterior")
    //            {
    //                cameraFieldOfView.soundController.PlayAudioClip("Confirm", obj.transform.position);
    //            }
    //            else
    //            {
    //                cameraFieldOfView.soundController.PlayAudioClip("Sweet Notification", obj.transform.position);
    //            }

    //            string objname = objectsInFieldOfView[i].name;
    //            Debug.LogError("Sound of " + objname);

    //            //await Task.Delay((int)(delay * 500));
    //        }
    //    }

    //    foreach (GameObject obj in objectsInFieldOfView)
    //    {
    //        cameraFieldOfView.importanceValues[obj.name] = originalImportance; // Update the importance value in the dictionary
    //    }
    //}

    public async void TriggerCheckFoV()
    {
        await Task.Delay((int)(0));
        IsCheckFoV = true;
    }

    public void OnCheckFieldOfViewButtonClick()
    {
        if (IsCheckFoV)
        {
            // Disable camera movement while the button is being clicked
            cameraController.DisableCameraMovement();

            // Check objects in the camera's field of view
            cameraFieldOfView.CheckObjectsInFieldOfView();

            // Re-enable camera movement after the button click is handled
            cameraController.EnableCameraMovement();

            IsCheckFoV = false;
        }
    }
}
