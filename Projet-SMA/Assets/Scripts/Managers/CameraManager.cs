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
    private bool start = false;
    private AudioSource audioMusic;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
        mainCameraButton.interactable = false;
        mainCameraButton.onClick.AddListener(DisplayMainCamera);
        agentCameraButton.onClick.AddListener(DisplayAgentCamera);
        audioMusic = GetComponent<AudioSource>();
        audioMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
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
    }

    public void SwitchAgent(Agent agent)
    {
        currentAgent._camera.enabled = false;
        currentAgent._canvas.enabled = false;
        currentAgent = agent;
        DisplayAgentCamera();
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
            // currentCamera != mainCamera
            DisplayMainCamera();
        }
    }

    public void DisplayMainCamera()
    {
        mainCamera.enabled = true;
        mainCamera.depth = 0;
        currentCamera = mainCamera;
    }

    public void DisplayAgentCamera()
    {
        currentAgent._camera.enabled = true;
        currentAgent._canvas.enabled = true;
        mainCamera.depth = -1;
        currentCamera = currentAgent._camera;
        Debug.LogError(currentAgent._code);
        Debug.LogError("Display Agent");
    }
}
