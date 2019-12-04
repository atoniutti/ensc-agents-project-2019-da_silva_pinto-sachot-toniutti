using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Toggle buttons
    public GameObject _difficultyNormalText;
    public GameObject _difficultyNormalTextLINE;
    public GameObject _difficultyHardcoreText;
    public GameObject _difficultyHardcoreTextLINE;
    public GameObject _panelDifficulty;

    public GameObject _yesText;
    public GameObject _yesTextLINE;
    public GameObject _noText;
    public GameObject _noTextLINE;

    // Sliders
    public GameObject _musicSlider;
    private float _sliderMusicValue = 0.0f;

    public void Start()
    {
        // Choice of difficulty
        if (PlayerPrefs.GetInt("Normal") == 1)
        {
            _difficultyNormalTextLINE.gameObject.SetActive(true);
            _difficultyHardcoreTextLINE.gameObject.SetActive(false);
            _panelDifficulty.SetActive(false);
        }
        else
        {
            _difficultyHardcoreTextLINE.gameObject.SetActive(true);
            _difficultyNormalTextLINE.gameObject.SetActive(false);
            _panelDifficulty.SetActive(true);
        }

        // Presence of sound effect
        if (PlayerPrefs.GetInt("Yes") == 1)
        {
            _yesTextLINE.gameObject.SetActive(true);
            _noTextLINE.gameObject.SetActive(false);
        }
        else
        {
            _noTextLINE.gameObject.SetActive(true);
            _yesTextLINE.gameObject.SetActive(false);
        }

        // Choice of sound level
        _musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void Update()
    {
        _sliderMusicValue = _musicSlider.GetComponent<Slider>().value;
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue / 4);
    }

    public void NormalDifficulty()
    {
        _difficultyNormalText.GetComponent<Text>().text = "Normal";
        _difficultyHardcoreText.GetComponent<Text>().text = "Difficult";
        _difficultyHardcoreTextLINE.gameObject.SetActive(false);
        _difficultyNormalTextLINE.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Normal", 1);
        PlayerPrefs.SetInt("Difficult", 0);

        _panelDifficulty.SetActive(false);
    }

    public void HardcoreDifficulty()
    {
        _difficultyNormalText.GetComponent<Text>().text = "Normal";
        _difficultyHardcoreText.GetComponent<Text>().text = "Difficult";
        _difficultyHardcoreTextLINE.gameObject.SetActive(true);
        _difficultyNormalTextLINE.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Normal", 0);
        PlayerPrefs.SetInt("Difficult", 1);
        _panelDifficulty.SetActive(true);
    }

    public void YesEffect()
    {
        _yesText.GetComponent<Text>().text = "Yes";
        _noText.GetComponent<Text>().text = "No";
        _noTextLINE.gameObject.SetActive(false);
        _yesTextLINE.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Yes", 1);
        PlayerPrefs.SetInt("No", 0);
    }

    public void NoEffect()
    {
        _yesText.GetComponent<Text>().text = "Yes";
        _noText.GetComponent<Text>().text = "No";
        _noTextLINE.gameObject.SetActive(true);
        _yesTextLINE.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Yes", 0);
        PlayerPrefs.SetInt("No", 1);
    }
}