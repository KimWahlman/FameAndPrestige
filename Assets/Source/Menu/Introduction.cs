using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour {
	public Button nextButton;
	public Button skipButton;
	public Button playButton;
	public Button backButton;
	public Text description;
	public GameObject images;
	public GameObject intro;
	public GameObject menu;
	public GameObject globalVar;
	private string externalurl = "http://ec2-52-17-161-123.eu-west-1.compute.amazonaws.com:8080/connect";
	private string localurl = "http://127.0.0.1:8080/connect";
	private int count = 0;

	private string[] descriptionArray = {"To create a work that will stand the test of time, one must know when to play their cards at the right time… Combine your cards to create a successful book. But, you need to figure out yourself which category the cards belong in. Everyone will have to use their wits when they are playing Fame and Prestige: The Romanticism Edition!"};


	void Start () {
		description.text = descriptionArray [count];
		images.SetActive(false);

		backButton.onClick.AddListener (goBack);
		nextButton.onClick.AddListener (Description);
	
		skipButton.onClick.AddListener (Skip);
		playButton.onClick.AddListener (LoadGame);
			}

	void TestOnClick(){
		Debug.Log ("clicked");
	}

	void goBack(){
		intro.SetActive (false);
		resetCanvas ();
		menu.SetActive(true);


	}

	private void resetCanvas(){
		count = 0;
		images.SetActive (false);
		playButton.interactable = false;
		skipButton.interactable = true;
		nextButton.interactable = true;
		description.text = descriptionArray [count];

	}


	IEnumerator requestPort() {
		WWW www = new WWW(localurl);
		yield return www;
		Debug.Log (www.text);
		JSONObject jso = new JSONObject (www.text);
		Debug.Log (jso["port"]);
		int port;
		int.TryParse(jso ["port"].ToString(), out port);
		globalVar.GetComponent<globalvar>().port = port;
		SceneManager.LoadScene ("main", LoadSceneMode.Single);

	}

	void LoadGame(){
		StartCoroutine(requestPort());
		//ui.SetActive(false);

	}

	void Skip(){
		playButton.interactable = true;
		nextButton.interactable = false;
		skipButton.interactable = false;
	}

	void Description(){
		count++;
		if (count >= descriptionArray.Length - 1) {
			nextButton.interactable = false;
			images.SetActive (true);
			playButton.interactable = true;
			skipButton.interactable = false; 
		}
		description.text = descriptionArray [count];


		

	}
}
