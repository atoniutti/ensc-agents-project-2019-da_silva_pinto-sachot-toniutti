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
    public AgentManager agentManager;

    private AudioSource audioMusic;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
        mainCameraButton.interactable = false;
        mainCameraButton.onClick.AddListener(DisplayMainCamera);
        agentCameraButton.onClick.AddListener(DisplayAgentCamera);
        audioMusic = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        audioMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
        if (mainCamera == currentCamera)
        {
            currentAgent = agentManager.agents[0];
        }
        
        //if (mainCamera == currentCamera)
        //{
        //    mainCameraButton.interactable = false;
        //    agentCameraButton.interactable = true;
        //}
        //else
        //{
        //    mainCameraButton.interactable = true;
        //    agentCameraButton.interactable = false;
        //}
    }

    public void SwitchCamera()
    {
        currentCamera.enabled = false;

        if (currentCamera == mainCamera)
        {
            DisplayAgentCamera();
        }
        else
        {
            // currentCamera == currentAgent._camera
            DisplayMainCamera();
        }
    }

    public void DisplayMainCamera()
    {
        mainCamera.enabled = true;
        currentCamera = mainCamera;
    }

    public void DisplayAgentCamera()
    {
        currentAgent._camera.enabled = true;
        currentCamera = currentAgent._camera;
    }
}
