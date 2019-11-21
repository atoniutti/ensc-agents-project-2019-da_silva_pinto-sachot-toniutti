using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public Button currentButton;
    public Button otherButton;

    public void SetCurrentCamera ()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();s
        cameraManager.SwitchCamera();

        // Set Interactable to false
        currentButton.interactable = false;
        otherButton.interactable = true;
    }
}
