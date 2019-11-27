using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Toggle buttons
    public GameObject difficultynormaltext;
    public GameObject difficultynormaltextLINE;
    public GameObject difficultyhardcoretext;
    public GameObject difficultyhardcoretextLINE;
    public GameObject panelDifficulty;

    public GameObject yestext;
    public GameObject yestextLINE;
    public GameObject notext;
    public GameObject notextLINE;

    // Sliders
    public GameObject musicSlider;
    private float sliderMusicValue = 0.0f;

    public void Start()
    {
        // Choix de la difficultée
        if (PlayerPrefs.GetInt("Normal") == 1)
        {
            difficultynormaltextLINE.gameObject.SetActive(true);
            difficultyhardcoretextLINE.gameObject.SetActive(false);
            panelDifficulty.SetActive(false);
        }
        else
        {
            difficultyhardcoretextLINE.gameObject.SetActive(true);
            difficultynormaltextLINE.gameObject.SetActive(false);
            panelDifficulty.SetActive(true);
        }

        // Présence d'effet sonor
        if (PlayerPrefs.GetInt("Yes") == 1)
        {
            yestextLINE.gameObject.SetActive(true);
            notextLINE.gameObject.SetActive(false);
        }
        else
        {
            notextLINE.gameObject.SetActive(true);
            yestextLINE.gameObject.SetActive(false);
        }

        // Choix du niveau sonor
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void Update()
    {
        sliderMusicValue = musicSlider.GetComponent<Slider>().value;
    }

    public void MusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", sliderMusicValue / 4);
    }

    public void NormalDifficulty()
    {
        difficultynormaltext.GetComponent<Text>().text = "Normal";
        difficultyhardcoretext.GetComponent<Text>().text = "Difficult";
        difficultyhardcoretextLINE.gameObject.SetActive(false);
        difficultynormaltextLINE.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Normal", 1);
        PlayerPrefs.SetInt("Difficult", 0);

        panelDifficulty.SetActive(false);
    }

    public void HardcoreDifficulty()
    {
        difficultynormaltext.GetComponent<Text>().text = "Normal";
        difficultyhardcoretext.GetComponent<Text>().text = "Difficult";
        difficultyhardcoretextLINE.gameObject.SetActive(true);
        difficultynormaltextLINE.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Normal", 0);
        PlayerPrefs.SetInt("Difficult", 1);
        panelDifficulty.SetActive(true);
    }

    public void YesEffect()
    {
        yestext.GetComponent<Text>().text = "Yes";
        notext.GetComponent<Text>().text = "No";
        notextLINE.gameObject.SetActive(false);
        yestextLINE.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Yes", 1);
        PlayerPrefs.SetInt("No", 0);
    }

    public void NoEffect()
    {
        yestext.GetComponent<Text>().text = "Yes";
        notext.GetComponent<Text>().text = "No";
        notextLINE.gameObject.SetActive(true);
        yestextLINE.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Yes", 0);
        PlayerPrefs.SetInt("No", 1);
    }
}