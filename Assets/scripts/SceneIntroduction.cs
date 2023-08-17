using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

public class SceneIntroduction : MonoBehaviour
{
    CameraFieldOfView cameraFieldOfView;
    CameraController cameraController;
    ButtonCheckFieldOfView buttonCheckFieldOfView;
    public string csvFilePath;  // Path to the CSV file
    private Camera mainCamera;
    private Dictionary<string, int> headerIndices;
    private List<string[]> csvData;
    private bool prevSecondaryButtonState_ = false;
    private bool isSpeaking_;
    private string description;
    private string cameraAnchor;
    private SpeechSynthesizer speechSynthesizer;

    void Start()
    {
        SpeechConfig speechConfig = SpeechConfig.FromSubscription("4de1d19d8bfe4fae9f46a2a3e848d548", "uksouth");

        // Create SpeechSynthesizer instance
        speechSynthesizer = new SpeechSynthesizer(speechConfig);
        cameraFieldOfView = GetComponent<CameraFieldOfView>();
        cameraController = GetComponent<CameraController>();
        buttonCheckFieldOfView = GetComponent<ButtonCheckFieldOfView>();
        mainCamera = Camera.main;
        isSpeaking_ = false;

        // Parse the CSV file
        ParseCSV();

        // Check for specific values and print the description
        //CheckAndPrintDescription(-0.55f, 1f, -0.9f, 45f);      

    }

    private async void Update()
    {
        if (cameraFieldOfView.leftsecondaryButtonDown && !prevSecondaryButtonState_)
        {
            if (buttonCheckFieldOfView.beginnerMode)
            {
                cameraFieldOfView.SpeakText("Left secondary button pressed!");
                await Task.Delay((int)(0));
                cameraFieldOfView.SpeakText("Scene Description function activated!");
                await Task.Delay((int)(0));
            }
            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 cameraRotation = mainCamera.transform.eulerAngles;
            CheckAndPrintDescription(cameraPosition.x, cameraPosition.y, cameraPosition.z, cameraRotation.y);
        }

        // Check if the primary button is pressed to cancel speech
        if (cameraFieldOfView.leftprimaryButtonDown && isSpeaking_)
        {
            CancelSpeech();
        }

        prevSecondaryButtonState_ = cameraFieldOfView.leftsecondaryButtonDown;
    }

    void ParseCSV()
    {
        csvData = new List<string[]>();

        // Read the CSV file
        using (StreamReader reader = new StreamReader(csvFilePath))
        {
            // Read header line
            string headerLine = reader.ReadLine();
            if (headerLine != null)
            {
                string[] headers = headerLine.Split(',');
                headerIndices = new Dictionary<string, int>();

                // Store the indices of each header
                for (int i = 0; i < headers.Length; i++)
                {
                    headerIndices[headers[i]] = i;
                }
            }

            // Read data lines
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] values = line.Split(',');
                    csvData.Add(values);
                }
            }
        }
    }

    private async void CheckAndPrintDescription(float posX, float posY, float posZ, float rotY)
    {
        int anchorIndex = headerIndices["Camera Anchor"];
        int posXIndex = headerIndices["PositionX"];
        int posYIndex = headerIndices["PositionY"];
        int posZIndex = headerIndices["PositionZ"];
        int rotYIndex = headerIndices["RotationY"];
        int descriptionIndex = headerIndices["Description"];
        var prev_diff = 100000000f;

        // Iterate through the CSV data
        for (int i = 0; i < csvData.Count; i++)
        {
            string[] row = csvData[i];
            float.TryParse(row[posXIndex], out float x);
            float.TryParse(row[posYIndex], out float y);
            float.TryParse(row[posZIndex], out float z);
            float.TryParse(row[rotYIndex], out float rot);
            var difference = (rotY - rot) * (rotY - rot) + 0.8 * ((posX - x) * (posX - x) + (posY - y) * (posY - y) + (posZ - z) * (posZ - z));
            if (difference < prev_diff)
            {
                description = row[descriptionIndex];
                cameraAnchor = row[anchorIndex];
                prev_diff = (float)difference;
            }
        }
        Debug.Log("Matching Camera Anchor:" + cameraAnchor);
        await Task.Delay((int)(0));
        cameraFieldOfView.SpeakText(description);
        // Start the speech service
        
        //await StartSpeech(description);
    }
    private async Task StartSpeech(string text)
    {
        // Set the speech synthesizer properties and start speaking
        speechSynthesizer.SynthesisStarted += SpeechSynthesizer_SynthesisStarted;
        speechSynthesizer.SynthesisCompleted += SpeechSynthesizer_SynthesisCompleted;
        await speechSynthesizer.SpeakTextAsync(text);
    }

    private void SpeechSynthesizer_SynthesisStarted(object sender, System.EventArgs e)
    {
        // Speech synthesis has started
        isSpeaking_ = true;
    }

    private void SpeechSynthesizer_SynthesisCompleted(object sender, System.EventArgs e)
    {
        // Speech synthesis has completed
        isSpeaking_ = false;
    }

    private void CancelSpeech()
    {
        // Cancel the speech synthesis
        speechSynthesizer.SynthesisStarted -= SpeechSynthesizer_SynthesisStarted;
        speechSynthesizer.SynthesisCompleted -= SpeechSynthesizer_SynthesisCompleted;
        speechSynthesizer.StopSpeakingAsync();
        isSpeaking_ = false;
    }

}
