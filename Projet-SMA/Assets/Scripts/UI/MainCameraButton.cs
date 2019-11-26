using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraButton : MonoBehaviour
{
    public void SwitchToMainCamera()
    {
        CameraButton mainCameraButton = GameObject.FindGameObjectWithTag("MainCameraButton").GetComponent<CameraButton>();
        mainCameraButton.SetCurrentCamera();
    }
}
