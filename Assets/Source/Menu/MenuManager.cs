using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	public Button startButton;
	public Button otherButton;
	public Button exitButton;
	public Button tutorialButton;
	public GameObject menu;
	public GameObject introduction;
	public GameObject tutorial;

	void Start () {
		menu.SetActive (true);
		Button st = startButton.GetComponent<Button>();
		st.onClick.AddListener(loadGame);

		Button ot = otherButton.GetComponent<Button> ();
		ot.onClick.AddListener (testOnClick);

		Button ex = exitButton.GetComponent<Button> ();
		ex.onClick.AddListener (gameShutDown);

		Button tu = tutorialButton.GetComponent<Button>();
		tu.onClick.AddListener (loadTutorial);
	}

	void testOnClick(){
		Debug.Log ("clicked");
	}

	void loadGame(){
		//SceneManager.LoadScene ("introduction", LoadSceneMode.Single);
		menu.SetActive(false);
		introduction.SetActive(true);
	}

	void loadTutorial(){
		menu.SetActive (false);
		tutorial.SetActive (true);
	
	}

	void gameShutDown(){
		Application.Quit();
	
	}
}
