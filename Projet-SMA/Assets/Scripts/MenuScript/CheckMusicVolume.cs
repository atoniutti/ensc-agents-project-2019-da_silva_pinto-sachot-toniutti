using UnityEngine;
using System.Collections;

public class CheckMusicVolume : MonoBehaviour
{
    public void Start()
    {
        // Allows you to remember the sound level of the last game session
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void UpdateVolume()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}