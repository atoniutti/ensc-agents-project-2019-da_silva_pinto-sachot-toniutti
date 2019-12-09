using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Toggle buttons
    public GameObject _yesText;
    public GameObject _yesTextLINE;
    public GameObject _noText;
    public GameObject _noTextLINE;

    // Sliders
    public GameObject _agentsSlider;
    public GameObject _energySlider;
    public GameObject _wasteSlider;
    public GameObject _musicSlider;
    public Text _energyText;
    public Text _wasteText;
    private int _sliderAgents = 3;
    private float _sliderEnergyValue = 50.0f;
    private float _sliderWasteValue = 30.0f;
    private float _sliderMusicValue = 0.5f;

    public void Start()
    {
        // Presence of sound effect
        YesEffect();

        // Number of Agents Slider
        PlayerPrefs.SetInt("NumberOfAgents", _sliderAgents);
        _agentsSlider.GetComponent<Slider>().value = _sliderAgents;

        // Energy percent Slider
        PlayerPrefs.SetFloat("EnergyPercent", _sliderEnergyValue);
        _musicSlider.GetComponent<Slider>().value = _sliderEnergyValue;

        // Waste Percent Slider
        PlayerPrefs.SetFloat("WastePercent", _sliderWasteValue);
        _musicSlider.GetComponent<Slider>().value = _sliderWasteValue;

        // Choice of sound level
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue);
        _musicSlider.GetComponent<Slider>().value = _sliderMusicValue;
    }

    public void Update()
    {
        _sliderAgents = (int)_agentsSlider.GetComponent<Slider>().value;
        _sliderEnergyValue = _energySlider.GetComponent<Slider>().value;
        _sliderWasteValue = _wasteSlider.GetComponent<Slider>().value;
        _sliderMusicValue = _musicSlider.GetComponent<Slider>().value;
    }

    public void AgentsSlider()
    {
        PlayerPrefs.SetInt("NumberOfAgents", _sliderAgents);
    }

    public void EnergySlider()
    {
        PlayerPrefs.SetFloat("EnergyPercent", _sliderEnergyValue);
        _energyText.text = _sliderEnergyValue.ToString() + "%";
    }

    public void WasteSlider()
    {
        PlayerPrefs.SetFloat("WastePercent", _sliderWasteValue);
        _wasteText.text = _sliderWasteValue.ToString() + "%";
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue);
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
