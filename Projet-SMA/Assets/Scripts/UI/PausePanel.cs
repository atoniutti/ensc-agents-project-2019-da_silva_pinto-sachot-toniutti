using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public GameObject _musicSlider;

    // Canvas
    public GlobalCanvas _globalCanvas;

    // Environment
    public CylinderLevel _energyLevel;
    public CylinderLevel _wasteLevel;

    // Slider
    public Slider _energySlider;
    public Slider _wasteSlider;

    // Text
    public Text _energyText;
    public Text _wasteText;

    private float _sliderMusicValue;

    // Start is called before the first frame update
    void Start()
    {
        ClosePanel();

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
        _energyText.text = _energySlider.value + "%";
        _wasteSlider.value = _wasteLevel._currentPercent;
        _wasteText.text = _wasteSlider.value + "%";
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        _globalCanvas.SetGameIsPaused(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        _globalCanvas.SetGameIsPaused(false);
    }

    public void QuitGame()
    {
        _globalCanvas.QuitGame();
    }
}
