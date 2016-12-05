using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class Tutorial : MonoBehaviour {
	
	public Button backButton;
	public GameObject menu;
	public GameObject tutorial;
	public MovieTexture movie;
	public AudioSource audio;
	public RawImage img;

	// Use this for initialization
	void Start () {
		backButton.onClick.AddListener (goBack);
		img.texture = movie as MovieTexture;
		audio = img.GetComponent<AudioSource> ();
		audio.clip = movie.audioClip;
		movie.Play ();
		audio.Play ();
	
	}

	void goBack(){
		tutorial.SetActive (false);
		menu.SetActive(true);
	
	}

}
