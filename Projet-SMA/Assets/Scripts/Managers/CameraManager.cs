using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Button _mainCameraButton;
    public Button _agentCameraButton;
    public Camera _mainCamera;
    public Camera _currentCamera;
    public Agent _currentAgent;
    public AgentManager _agentManager;
    private bool _start = false;
    private AudioSource _audioMusic;

    // Start is called before the first frame update
    void Start()
    {
        _currentCamera = _mainCamera;
        _mainCameraButton.interactable = false;
        _mainCameraButton.onClick.AddListener(DisplayMainCamera);
        _agentCameraButton.onClick.AddListener(DisplayAgentCamera);
        _audioMusic = GetComponent<AudioSource>();
        _audioMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    // Update is called once per frame
    void Update()
    {
        // First Initialisation after starting project
        if (!_start)
        {
            // Initiate Main Camera
            DisplayMainCamera();

            // Initiate Current Agents
            _currentAgent = _agentManager._agents[0];

            _start = true;
        }
    }

    public void SwitchAgent(Agent agent)
    {
        _currentAgent._camera.enabled = false;
        _currentAgent._canvas.enabled = false;
        _currentAgent = agent;
        DisplayAgentCamera();
    }

    public void SwitchCamera()
    {
        _currentCamera.enabled = false;

        if (_currentCamera == _mainCamera)
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
        _mainCamera.enabled = true;
        _mainCamera.depth = 0;
        _currentCamera = _mainCamera;
    }

    public void DisplayAgentCamera()
    {
        _currentAgent._camera.enabled = true;
        _currentAgent._canvas.enabled = true;
        _mainCamera.depth = -1;
        _currentCamera = _currentAgent._camera;
    }
}
