using System;
using System.Collections.Generic;
using System.Media;
using UnityEngine;
using System.Threading.Tasks;
using JetBrains.Annotations;

public class ButtonCheckFieldOfView : MonoBehaviour
{
    CameraFieldOfView cameraFieldOfView;
    CameraController cameraController;
    bool IsCheckFoV = false;
    private bool prevTriggerButtonState_ = false;
    private bool prevPrimaryButtonState_ = false;
    private bool prevSecondaryButtonState_ = false;
    private bool raySweepState_ = false;
    public Transform controller;
    private List<GameObject> raySweepList = new List<GameObject>();
    private string raySweepListName;
    private GameObject prevObj;
    private GameObject recentlySpokenObject;
    private bool localizationMode = false;
    private bool readFast = true;
    private RepeatingSoundPlayer soundPlayer;
    public bool beginnerMode = false;

    void Start()
    {
        cameraFieldOfView = GetComponent<CameraFieldOfView>();
        cameraController = GetComponent<CameraController>();
        GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>();
        soundPlayer = GameObject.Find("BeepAudioSource").GetComponent<RepeatingSoundPlayer>();
    }

    async void Update()
    {
        if (cameraFieldOfView.triggerButtonDown && !prevTriggerButtonState_)
        {
            raySweepList = new List<GameObject>();
            raySweepListName = string.Empty;
        }
        if (cameraFieldOfView.triggerButtonDown)
        {
            Vector3 controllerDirection = controller.forward;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(controller.position, controllerDirection, out hit, 10f);
            if (hasHit)
            {
                GameObject hitobj = hit.transform.gameObject;
                if (raySweepList == null)
                {
                    raySweepList.Add(hitobj);
                    raySweepListName = raySweepListName + ", " + hitobj.name;
                    GameObject prevObj = hitobj;
                }
                else
                {
                    if (hitobj != prevObj)
                    {
                        raySweepList.Add(hitobj);
                        raySweepListName = raySweepListName + ", " + hitobj.name;
                        prevObj = hitobj;
                    }
                }
            }
        }

        if (cameraFieldOfView.primaryButtonDown && !prevPrimaryButtonState_)
        {
            if (beginnerMode)
            {
                cameraFieldOfView.SpeakText("Right primary button pressed!");
                await Task.Delay((int)(0));
                beginnerMode = false;
                cameraFieldOfView.SpeakText("Beginner Mode Deactivated!");
            }
            else
            {
                beginnerMode = true;
                cameraFieldOfView.SpeakText("Beginner Mode Activated!");
            }
        }

        if (cameraFieldOfView.secondaryButtonDown && !prevSecondaryButtonState_)
        {
            if (beginnerMode)
            {
                cameraFieldOfView.SpeakText("Right secondary button pressed!");
                await Task.Delay((int)(0));
                cameraFieldOfView.SpeakText("Object Searching Function Deactivated!");
            }
            localizationMode = false;
            soundPlayer.DeactivateBeep();
        }

        if (!cameraFieldOfView.triggerButtonDown && prevTriggerButtonState_)
        {
            ReadRaySweepList(raySweepList);
        }
        prevTriggerButtonState_ = cameraFieldOfView.triggerButtonDown;
        prevPrimaryButtonState_ = cameraFieldOfView.primaryButtonDown;
        prevSecondaryButtonState_ = cameraFieldOfView.secondaryButtonDown;
    }

    public async void ReadRaySweepList(List<GameObject> raySweepList)
    {
        if (raySweepList.Count>1)
        //if (readFast)
        {
            if (beginnerMode)
            {
                cameraFieldOfView.SpeakText("Right trigger pressed!");
                await Task.Delay((int)(0));
                cameraFieldOfView.SpeakText("Ray Sweeping Function Activated!");
            }
            Debug.Log(raySweepListName);
            cameraFieldOfView.SpeakText(raySweepListName);
        }
        else
        {
            if (beginnerMode)
            {
                cameraFieldOfView.SpeakText("Right trigger pressed!");
                await Task.Delay((int)(0));
                cameraFieldOfView.SpeakText("Raycast Object Searching Function Activated!");
            }
            Vector3 controllerDirection = controller.forward;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(controller.position, controllerDirection, out hit, 10f);
            GameObject hitobj = hit.transform.gameObject;
            string hitobjname = hit.transform.name;
            await Task.Delay((int)(0));
            cameraFieldOfView.SpeakText(hitobjname.Replace("_", " "));
            await Task.Delay((int)(2000));
            soundPlayer.TriggerBeep(hitobj.transform, "right");
         
        }
    }
}

