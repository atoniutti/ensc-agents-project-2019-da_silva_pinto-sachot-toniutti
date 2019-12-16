using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string _sceneName = "";
    public Light _lightMenu;
    public GameObject _panelGame;
    public GameObject _panelareYouSure;
    public Material _skyBox;

    Animator _cameraObject;

    void Start()
    {
        _cameraObject = transform.GetComponent<Animator>();
        _lightMenu.intensity = 2;
        RenderSettings.skybox = _skyBox;
    }

    public void Play()
    {
        _panelareYouSure.gameObject.SetActive(false);
        NewGame();
    }

    public void NewGame()
    {
        if (_sceneName != "")
        {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }
    }

    public void Position1()
    {
        _lightMenu.intensity = 2;
        _cameraObject.SetFloat("Animate", 0);
    }

    public void Position2()
    {
        _lightMenu.intensity = 2;

        _panelareYouSure.gameObject.SetActive(false);
        _cameraObject.SetFloat("Animate", 1);
    }

    // Are You Sure - Quit Panel Pop Up
    public void AreYouSure()
    {
        _panelareYouSure.gameObject.SetActive(true);
    }

    public void NoExit()
    {
        _panelareYouSure.gameObject.SetActive(false);
    }

    public void YesExit()
    {
        Application.Quit();
    }
}