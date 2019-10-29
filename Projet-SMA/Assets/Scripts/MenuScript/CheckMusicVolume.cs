using UnityEngine;
using System.Collections;

public class CheckMusicVolume : MonoBehaviour {
    
	public void  Start (){
		// Permet de se rappeler du niveau sonor de la derniere session de jeu
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
	}

	public void UpdateVolume (){
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
	}

}