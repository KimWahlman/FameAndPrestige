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
	public string[] namePainter;
	public GameObject informationList;
	public GameObject h1preb;
	public GameObject h2preb;
	public GameObject h3preb;
	public GameObject parpreb;
	public GameObject content;


	private string url = "https://en.wikipedia.org/w/api.php?format=json&formatversion=2&action=query&prop=extracts&exintro=&explaintext=&titles=";
	private GameObject element;
	private GameObject desPainting;
	private GameObject desPainter;

	private string infPaint = "";
	private string infAuth = "";


	// Use this for initialization
	IEnumerator Start () {

		cardButton.onClick.AddListener (showInf);
		StartCoroutine(LoadInformation ());
		yield return 0;

	
	}



	IEnumerator LoadInformation(){

		element = (GameObject)Instantiate (content,new Vector3(0,0,0), Quaternion.identity);
		GameObject title = (GameObject)Instantiate (h1preb, new Vector3(0,0,0), Quaternion.identity);
		GameObject imgHeading =  (GameObject)Instantiate (h2preb,new Vector3(0,0,0), Quaternion.identity);

		GameObject headingPaint =(GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
		desPainting = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);

		GameObject headingPainter = (GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
		desPainter = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);

		string str = WWW.UnEscapeURL(namePainting [0]);
		str = str.Replace (" ","_");
		url = url + str;
		WWW www = new WWW(url);
		yield return www;
		JSONObject jso = new JSONObject (www.text);


		string str2 = WWW.UnEscapeURL (namePainter [0]);
		str2 = str.Replace (" ","_");
		string url2 = url + str2;
		WWW www2 = new WWW(url2);
		yield return www2;
		JSONObject jso2 = new JSONObject (www2.text);
		Debug.Log (jso);



		if (!jso ["query"] ["pages"].HasField ("-1")) {
			
			Debug.Log ("missing");
		} else if (!jso2 ["query"] ["pages"].HasField ("-1")) {	
			Debug.Log ("missing");
		} else {
			this.infPaint = jso["query"]["pages"][0]["extract"].ToString();
			this.infAuth = jso2["query"]["pages"][0]["extract"].ToString();
		}

		element.name = name;

		title.GetComponent<Text> ().text = titleCard;
		imgHeading.GetComponent<Text>().text = "Image information";
		desPainting.GetComponent<Text> ().text = infPaint.ToString();
		headingPaint.GetComponent<Text> ().text = "\""+namePainting [0]+"\"";
		headingPainter.GetComponent<Text> ().text = "Painted by \n"+namePainter [0];
		//desPainter.GetComponent<Text> ().text = infAuth.ToString();

		title.transform.SetParent (element.transform,false);
		imgHeading.transform.SetParent (element.transform, false);
		headingPaint.transform.SetParent (element.transform, false);
		desPainting.transform.SetParent(element.transform, false);
		headingPainter.transform.SetParent(element.transform, false);
		desPainter.transform.SetParent(element.transform, false);
	
		element.transform.SetParent (informationList.GetComponentInChildren<ScrollRect> ().content, false);
		element.SetActive (false);

	}

	void showInf(){
		
		ScrollRect tmp = informationList.GetComponentInChildren<ScrollRect> ();

		for( int i = 0; i <tmp.content.transform.childCount; ++i )
		{
			tmp.content.transform.GetChild(i).gameObject.SetActive(false);
		}
	
		element.SetActive (true);



	}



}
