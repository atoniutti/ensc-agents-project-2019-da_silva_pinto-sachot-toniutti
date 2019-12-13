using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public GameObject _musicSlider;
    public string _menuSceneName = "";

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
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusicValue);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        if (_menuSceneName != "")
        {
            SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Single);
        }
    }
}
