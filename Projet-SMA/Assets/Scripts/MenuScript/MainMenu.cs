using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator CameraObject;
    public string sceneName = "";
    public Light lightMenu;
    public GameObject panelGame;
    public GameObject panelareYouSure;
    public Material skyBox;

    void Start()
    {
        CameraObject = transform.GetComponent<Animator>();
        lightMenu.intensity = 2;
        RenderSettings.skybox = skyBox;
    }

    public void Play()
    {
        panelareYouSure.gameObject.SetActive(false);
        NewGame();
    }
    
    public void NewGame()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }

    public void DisablePlay()
    {

    }

    public void Position1()
    {
        lightMenu.intensity = 2;
        CameraObject.SetFloat("Animate", 0);
    }

    public void Position2()
    {
        lightMenu.intensity = 2;
        DisablePlay();

        panelareYouSure.gameObject.SetActive(false);
        CameraObject.SetFloat("Animate", 1);
    }
    
    // Are You Sure - Quit Panel Pop Up
    public void AreYouSure()
    {
        panelareYouSure.gameObject.SetActive(true);
        DisablePlay();
    }

    public void NoExit()
    {
        panelareYouSure.gameObject.SetActive(false);
    }

    public void YesExit()
    {
        Application.Quit();
    }
}