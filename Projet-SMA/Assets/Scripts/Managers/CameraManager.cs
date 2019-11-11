using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Button mainCameraButton;
    public Button agentCameraButton;
    public Camera mainCamera;
    public Camera currentCamera;
    public Agent currentAgent;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
        currentAgent = FindObjectOfType<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == currentCamera)
        {
            mainCameraButton.interactable = false;
            agentCameraButton.interactable = true;
        }
        else
        {
            mainCameraButton.interactable = true;
            agentCameraButton.interactable = false;
        }
    }
}
