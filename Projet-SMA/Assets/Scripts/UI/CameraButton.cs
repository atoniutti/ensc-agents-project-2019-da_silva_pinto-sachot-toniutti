using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public void SwitchCamera()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
        cameraManager.SwitchCamera();
    }
}
