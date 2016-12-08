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
	private GameObject desPainting;
	private GameObject element;



	// Use this for initialization
	IEnumerator Start () {

		cardButton.onClick.AddListener (testOnClick);
		StartCoroutine(LoadInformation ());
		yield return 0;

	
	}



	IEnumerator LoadInformation(){
		
		GameObject imgHeading =  (GameObject)Instantiate (h2preb,new Vector3(0,0,0), Quaternion.identity);  
		element = (GameObject)Instantiate (content,new Vector3(0,0,0), Quaternion.identity);
		GameObject title = (GameObject)Instantiate (h1preb, new Vector3(0,0,0), Quaternion.identity);
		GameObject  headingPaint =(GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
		desPainting = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);

		string str = namePainting [0];
		str = str.Replace (" ","_");
		url = url + str;
		//Debug.Log (url);
		WWW www = new WWW(url);
		yield return www;

		JSONObject jso = new JSONObject (www.text);
	

		if (jso ["query"] ["pages"].HasField ("-1")) {
			//Debug.Log ("missing");		



		}
		else {

			infPaint = jso["query"]["pages"][0]["extract"].ToString();

		}
		//Debug.Log (infPaint.ToString());



		element.name = name;

		title.GetComponent<Text> ().text = titleCard;
		imgHeading.GetComponent<Text>().text = "Image information";
		desPainting.GetComponent<Text> ().text = infPaint.ToString();
		headingPaint.GetComponent<Text> ().text = namePainting [0];

		//Debug.Log (desPainting.GetComponent<Text> ().text);

		title.transform.SetParent (element.transform,false);
		imgHeading.transform.SetParent (element.transform, false);
		headingPaint.transform.SetParent (element.transform, false);
		desPainting.transform.SetParent(element.transform, false);

	
		element.transform.SetParent (informationList.GetComponentInChildren<ScrollRect> ().content, false);
		element.SetActive (false);

	}

	void testOnClick(){
		Debug.Log ("clicked");
		ScrollRect tmp = informationList.GetComponentInChildren<ScrollRect> ();

		for( int i = 0; i <tmp.content.transform.childCount; ++i )
		{
			tmp.content.transform.GetChild(i).gameObject.SetActive(false);
		}
		/*
		for (int i = 0; i < element.transform.childCount; ++i) {
			element.transform.GetChild (i).gameObject.SetActive (true);
		}*/
		element.SetActive (true);



	}

	/*
	IEnumerator requestInformation(string str, GameObject ob) {
		str = str.Replace (" ","_");
		url = url + str;
		//Debug.Log (url);
		WWW www = new WWW(url);
		yield return www;
		//Debug.Log (www.text);

		JSONObject jso = new JSONObject (www.text);
		//JSONObject id =  jso["query"]["pages"][0];
		Debug.Log (jso);
		Debug.Log (jso ["query"] ["pages"]);

		if (jso ["query"] ["pages"].HasField ("-1")) {
			//Debug.Log ("missing");		
		
		}
		else {
			//Debug.Log (jso);
			ob.GetComponent<Text>().text = jso["query"]["pages"][0]["extract"].ToString();
			//return infPaint;
			//Debug.Log (infPaint);
		}

	}
	*/

}
