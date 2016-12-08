using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShowInformation : MonoBehaviour {
	public Button cardButton;
	public string titleCard;
	public string name;
	private string type;
	public string[] namePainting;
	public string[] authoPainting;
	public GameObject informationList;
	public GameObject h1preb;
	public GameObject h2preb;
	public GameObject h3preb;
	public GameObject parpreb;
	public GameObject content;

	private string url = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=";
	private string infPaint = "";
	private string infAuth = "";



	// Use this for initialization
	void Start () {

		cardButton.onClick.AddListener (testOnClick);
		LoadInformation ();
	
	
	}

	void LoadInformation(){





		GameObject imgHeading =  (GameObject)Instantiate (h2preb,new Vector3(0,0,0), Quaternion.identity);  
		GameObject element = (GameObject)Instantiate (content,informationList.transform.position, informationList.transform.rotation);
		GameObject title = (GameObject)Instantiate (h1preb, new Vector3(0,0,0), Quaternion.identity);
		GameObject  headingPaint =(GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
		GameObject desPainting = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);
		StartCoroutine (requestInformation (namePainting[0], infPaint));


		element.name = name;

		title.GetComponent<Text> ().text = titleCard;
		imgHeading.GetComponent<Text>().text = "Image information";
		desPainting.GetComponent<Text> ().text = infPaint;
		//headingPaint.GetComponent<Text> ().text = namePainting [0];

		title.transform.SetParent (element.transform,false);
		imgHeading.transform.SetParent (element.transform, false);
		headingPaint.transform.SetParent (element.transform, false);
			





		
	
		element.transform.SetParent (informationList.GetComponentInChildren<ScrollRect> ().content, false);


		element.SetActive (false);

	}

	void testOnClick(){
		Debug.Log ("clicked");
	}



	IEnumerator requestInformation(string str, string infPaint) {
		str = str.Replace (" ","_");
		url = url + str;
		Debug.Log (url);
		WWW www = new WWW(url);

		//Debug.Log (www.text);
		JSONObject jso = new JSONObject (www.text);
		//JSONObject id =  jso["query"]["pages"][0];
		infPaint = jso["query"]["pages"][0]["extract"].ToString();
		yield return www;
		//Debug.Log (jso ["query"] ["pages"] [id] ["extract"].ToString());

	}
	

}
