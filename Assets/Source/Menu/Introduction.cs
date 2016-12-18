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
	private string url = "http://ec2-52-17-161-123.eu-west-1.compute.amazonaws.com:8080/connect";
	private int count = 0;

	private string[] descriptionArray = {"Fame and prestige to you, great author of Romanticism, writer of strong emotions.\nRomanticism" +
		" is a period of great changes and rebellions, the Europe need new operas to escape from the reality.\nThis movement " +
		"emphasized intense emotion as an authentic source of aesthetic experience, leaded by four principal themes " +
		"Folklore, Nature, History and Nature.",
		"Take the role as one of this famous authors and relieve the feeling of creating something great during the Romanticism era.\n" +
		"Who know which writer you will have...\nMary Shelley? Grimm Brothers? William Wordsworth? Bettina von Armin?\n",
		"Write about different theme,recount about legends using the  popular Folklore, glorify the History, admires wonders of the Nature, " +
		"recount scary and creepy Horror stories.\nYou are in competition against other tree writers " +
		"who must use their wits and knowledge to chain various words together to create their next masterpiece.\n" +
		"Can you create a masterpiece that will stand the test of time?\n",
		"This is \nFame and Prestige: The Romanticism Edition!\n"};


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
		WWW www = new WWW(url);
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
		if( count == 1)
			images.SetActive (true);

		if (count >= descriptionArray.Length - 1) {
			nextButton.interactable = false;
			playButton.interactable = true;
			skipButton.interactable = false; 
			images.SetActive (false);
		}
		description.text = descriptionArray [count];

		Debug.Log (count);
		

	}
}
