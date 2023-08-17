using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class CameraFieldOfView : MonoBehaviour
{
    public Camera mainCamera;
    ButtonCheckFieldOfView buttonCheckFieldOfView;
    public Dictionary<string, float> importanceValues;
    public Dictionary<string, float> newImportanceValues;
    public List<GameObject> readObjects;
    Dictionary<string, string> descriptions;
    SpeechSynthesizer synthesizer;
    private AudioSource audioSource;
    public bool localizationMode = false;
    public float volume = 1f;
    public float spatialBlend = 1f;
    public SpatialSoundController soundController;
    private RepeatingSoundPlayer soundPlayer;
    // Add a private variable to store the most recently spoken object
    public GameObject recentlySpokenObject;

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
    private float timer = 0f; // Timer to track the elapsed time

    void Start()
    {
        mainCamera = Camera.main;
        LoadImportanceValues();
        readObjects = new List<GameObject>();
        // Create SpeechConfig instance
        SpeechConfig speechConfig = SpeechConfig.FromSubscription("4de1d19d8bfe4fae9f46a2a3e848d548", "uksouth");

        // Create SpeechSynthesizer instance
        synthesizer = new SpeechSynthesizer(speechConfig);

        // Get the AudioSource component attached to the object
        audioSource = GetComponent<AudioSource>();
        soundController = GetComponent<SpatialSoundController>();
        
        soundPlayer = GameObject.Find("BeepAudioSource").GetComponent<RepeatingSoundPlayer>();
        buttonCheckFieldOfView = GetComponent<ButtonCheckFieldOfView>();
    }

    async void Update()
    {
        // Check if the button on the right hand controller is pressed
        primaryButtonDown = false;//OVRInput.Get(OVRInput.Button.One);
        rightHand.inputDevice.IsPressed(primaryButton, out primaryButtonDown);
        
        secondaryButtonDown = false;
        rightHand.inputDevice.IsPressed(secondaryButton, out secondaryButtonDown);
        
        triggerButtonDown = false;
        rightHand.inputDevice.IsPressed(triggerButton, out triggerButtonDown);
        
        leftprimaryButtonDown = false;
        leftHand.inputDevice.IsPressed(primaryButton, out leftprimaryButtonDown);
        
        leftsecondaryButtonDown = false;
        leftHand.inputDevice.IsPressed(secondaryButton, out leftsecondaryButtonDown);
        
        lefttriggerButtonDown = false;
        leftHand.inputDevice.IsPressed(triggerButton, out lefttriggerButtonDown);
    }

    public async void CheckObjectsInFieldOfView()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is not assigned!");
            return;
        }

        GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();

        List<GameObject> objectsInFieldOfView = new List<GameObject>();

        foreach (GameObject obj in objectsInScene)
        {
            // Get the object's transform component to retrieve its position
            Transform objTransform = obj.transform;
            Vector3 objPosition = objTransform.position;
            //Debug.LogError("Checking visibility of: " + obj.name);
            // Check if the object is within the camera's field of view
            if (IsObjectVisible(objPosition, mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.fieldOfView))
            {
                objectsInFieldOfView.Add(obj);
            }
        }

        // Calculate the new importance values for each object based on the distance from the camera
        foreach (GameObject obj in objectsInFieldOfView)
        {
            float originalImportance = GetImportanceValue(obj.name);
            float newImportance = GetNewImportanceValue(obj.name);
            float distance = Vector3.Distance(obj.transform.position, mainCamera.transform.position);
            newImportance = originalImportance * (Mathf.Exp(-distance) - Mathf.Exp(-5));
            int objectDecay = readObjects.Where(x => x.Equals(obj)).Count();
            newImportance = (float)(newImportance * Math.Pow(0.5, objectDecay));
            if (distance > 5)
                newImportance = 0;
            //importanceValues[obj.name] = newImportance; // Update the importance value in the dictionary
            newImportanceValues[obj.name] = newImportance;
        }

        // Sort the objects based on the new importance values
        objectsInFieldOfView.Sort((a, b) => GetNewImportanceValue(b.name).CompareTo(GetNewImportanceValue(a.name)));

        // Print the three objects with the highest new importance values
        for (int i = 0; i < Mathf.Min(1, objectsInFieldOfView.Count); i++)
        {
            GameObject obj = objectsInFieldOfView[i];
            if (GetNewImportanceValue(obj.name) > 0 && localizationMode == false)
            {
                string message = obj.name;
                string messageFull = obj.name + " is within the field of view. Position: " + obj.transform.position + " Importance: " + GetImportanceValue(obj.name);
                Debug.LogError(messageFull);
                SpeakText(message.Replace("_", " "));
                readObjects.Add(obj);

                // Store the most recently spoken object
                recentlySpokenObject = obj;

                // Delay before playing the audio clip
                float delay = 2.0f; // Adjust the delay time as needed
                if (buttonCheckFieldOfView.beginnerMode)
                    await Task.Delay((int)(delay * 4500));

                // Play the audio clip at the object's position
                string path = obj.name;
                Transform currentTransform = obj.transform;

                while (currentTransform.parent != null)
                {
                    currentTransform = currentTransform.parent;
                    path = currentTransform.name + "/" + path;
                }
                
                //Debug.LogError("Position for" + obj.name + ": " + obj.transform.position);
                if (currentTransform.name == "Interactables")
                {
                    soundController.PlayAudioClip("Positive Notification", obj.transform.position);
                }
                else if (currentTransform.name == "Interior")
                {
                    soundController.PlayAudioClip("Magic Spell", obj.transform.position);
                }
                else if (currentTransform.name == "Potions")
                {
                    soundController.PlayAudioClip("Magic", obj.transform.position);
                }
                else if (currentTransform.name == "Exterior")
                {
                    soundController.PlayAudioClip("Confirm", obj.transform.position);
                }
                else
                {
                    soundController.PlayAudioClip("Sweet Notification", obj.transform.position);
                }

                string objname = objectsInFieldOfView[i].name;
                Debug.LogError("Sound of " + objname);

                await Task.Delay((int)(delay * 500));

                soundPlayer.TriggerBeep(obj.transform, "right");

                //if (primaryButtonDown)
                //// if (XRControllerRightButtonPressed())
                //{
                //    string selectMessage = message + " Selected!";
                //    SpeakText(selectMessage.Replace("_", " "));
                //    Debug.LogError(message + " Selected!");
                //    if (recentlySpokenObject != null)
                //    {
                //        Debug.LogError("Playing sound to indicate distance between controller and virtual object...");
                //        localizationMode = true;
                //        soundPlayer.TriggerBeep(recentlySpokenObject.transform);
                        
                //        if (secondaryButtonDown)
                //        {
                //            //Debug.LogError("Right Controller Secondary button pressed!");
                //            localizationMode = false;
                //            soundPlayer.DeactivateBeep();
                //        }
                //    }
                //}
                //await Task.Delay((int)(delay * 300));
            }
        }

        await Task.Delay((int)(500f));
    }

    public async void SpeakText(string text)
    {
        await synthesizer.SpeakTextAsync(text);
    }

   

    public bool IsObjectVisible(Vector3 objectPosition, Vector3 cameraPosition, Quaternion cameraRotation, float fieldOfView)
    {
        // Calculate the direction from the camera to the object
        Vector3 cameraToObject = objectPosition - cameraPosition;

        // Calculate the angle between the camera's forward direction and the direction to the object
        float angle = Vector3.Angle(cameraRotation * Vector3.forward, cameraToObject);

        // Check if the angle is within the camera's field of view
        if (angle <= fieldOfView / 2f)
        {
            // Object is within the field of view
            return true;
        }

        // Object is outside the field of view
        return false;
    }

    void LoadImportanceValues()
    {
        string jsonFilePath = Path.Combine(Application.dataPath, "scene_graph_importance.json");
        //Debug.LogError("Importance Values Loaded from:" + jsonFilePath);

        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            SceneGraph sceneGraphContent = JsonConvert.DeserializeObject<SceneGraph>(jsonContent);

            if (sceneGraphContent != null)
            {
                importanceValues = new Dictionary<string, float>();
                newImportanceValues = new Dictionary<string, float>();
                //descriptions = new Dictionary<string, string>(); // New dictionary for descriptions
                TraverseSceneGraph(sceneGraphContent.children);

                Debug.LogError("Importance values and descriptions loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON content.");
            }
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + jsonFilePath);
        }
    }

    void TraverseSceneGraph(List<SceneGraph> children)
    {
        if (children == null || children.Count == 0)
            return;

        foreach (SceneGraph child in children)
        {
            if (child.importance.HasValue)
            {
                if (!importanceValues.ContainsKey(child.name))
                {
                    importanceValues.Add(child.name, child.importance.Value);
                }
                if (!newImportanceValues.ContainsKey(child.name))
                {
                    newImportanceValues.Add(child.name, child.importance.Value);
                }
            }

            TraverseSceneGraph(child.children);
        }
    }

    public float GetImportanceValue(string objectName)
    {
        if (importanceValues.ContainsKey(objectName))
        {
            return importanceValues[objectName];
        }

        //Debug.LogWarning("Importance value not found for object: " + objectName);
        return 0f;
    }

    public float GetNewImportanceValue(string objectName)
    {
        if (newImportanceValues.ContainsKey(objectName))
        {
            return newImportanceValues[objectName];
        }

        //Debug.LogWarning("Importance value not found for object: " + objectName);
        return 0f;
    }

    [System.Serializable]
    public class SceneGraph
    {
        public string name;
        public string type;
        public List<SceneGraph> children;
        public List<ComponentData> components;
        public float? importance;
        public string? description;
    }

    [System.Serializable]
    public class ComponentData
    {
        public string type;
        // Add other component properties as needed
    }
}