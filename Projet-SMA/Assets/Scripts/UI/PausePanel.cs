using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public GameObject _musicSlider;
    public string _menuSceneName = "";

    // Canvas
    public PauseCanvas _pauseCanvas;

    // Environment
    public CylinderLevel _energyLevel;
    public CylinderLevel _wasteLevel;

    // Slider
    public Slider _energySlider;
    public Slider _wasteSlider;

    private float _sliderMusicValue;

    // Start is called before the first frame update
    void Start()
    {
        // Choice of sound level
        _sliderMusicValue = PlayerPrefs.GetFloat("MusicVolume");
        _musicSlider.GetComponent<Slider>().value = _sliderMusicValue;
    }

    // Update is called once per frame
    void Update()
    {
        _sliderMusicValue = _musicSlider.GetComponent<Slider>().value;

        // Update Slider Of Energy & Waste Piles
        _energySlider.value = _energyLevel._currentPercent;
        _wasteSlider.value = _wasteLevel._currentPercent;
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        _pauseCanvas.SetGameIsPaused(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        _pauseCanvas.SetGameIsPaused(false);
    }

    public void QuitGame()
    {
        if (_menuSceneName != "")
        {
            SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Single);
        }
    }
}
