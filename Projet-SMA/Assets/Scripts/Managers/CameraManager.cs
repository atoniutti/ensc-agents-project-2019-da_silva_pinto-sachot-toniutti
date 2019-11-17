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
    private AudioSource audioMusic;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = mainCamera;
        currentAgent = FindObjectOfType<Agent>();
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
            mainCameraButton.interactable = false;
            agentCameraButton.interactable = true;
        }
        else
        {
            mainCameraButton.interactable = true;
            agentCameraButton.interactable = false;
        }
    }

    public void DisplayMainCamera()
    {
        currentCamera = mainCamera;
    }

    public void DisplayAgentCamera()
    {
        currentCamera = currentAgent._camera;
    }
}
