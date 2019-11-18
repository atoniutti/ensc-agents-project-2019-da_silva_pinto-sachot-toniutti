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
    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
        mainCameraButton.interactable = false;
        audioMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // First Initialisation after starting project
        if (!start)
        {
            // Initiate Main Camera
            DisplayMainCamera();

            // Initiate Current Agents
            currentAgent = agentManager.agents[0];

            start = true;
        }

        audioMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
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

    public void SwitchAgent(Agent agent)
    {
        currentCamera.enabled = false;
        currentAgent = agent;
        currentAgent._camera.enabled = true;
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
