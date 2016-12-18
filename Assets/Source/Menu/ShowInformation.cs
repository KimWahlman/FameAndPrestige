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
	public Texture[] imgSource;
	public GameObject informationList;
	public GameObject h1preb;
	public GameObject h2preb;
	public GameObject h3preb;
	public GameObject parpreb;
	public GameObject content;



	private string url = "https://en.wikipedia.org/w/api.php?format=json&formatversion=2&action=query&prop=extracts&exintro=&explaintext=&titles=";
	private GameObject element;



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

		element.name = name;
		title.GetComponent<Text> ().text = titleCard;
		imgHeading.GetComponent<Text>().text = "Image information";
		title.transform.SetParent (element.transform,false);
		imgHeading.transform.SetParent (element.transform, false);

		for(int i = 0; i< namePainting.Length;i++){
			GameObject headingPaint =(GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
			GameObject desPainting = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);

			GameObject headingPainter = (GameObject)Instantiate (h3preb,new Vector3(0,0,0), Quaternion.identity);
			GameObject desPainter = (GameObject)Instantiate (parpreb,new Vector3(0,0,0), Quaternion.identity);

			string str =namePainting [i];
			str = str.Replace (" ","_");
			string str2 = namePainter [i];
			str2 = str2.Replace (" ","_");

			string urlPainter = url + str2;
			string urlPainting = url + str;

			WWW www = new WWW(urlPainting);
			yield return www;
			JSONObject jso = new JSONObject (www.text);

			www = new WWW(urlPainter);
			yield return www;
			JSONObject jso2 = new JSONObject (www.text);


			if (!jso ["query"] ["pages"][0].HasField ("missing"))
				desPainting.GetComponent<Text> ().text = jso ["query"] ["pages"] [0] ["extract"].ToString ();
			else
				desPainting.GetComponent<Text> ().text = ""; 


			if (!jso2 ["query"] ["pages"][0].HasField ("missing"))
				desPainter.GetComponent<Text> ().text = jso2 ["query"] ["pages"] [0] ["extract"].ToString ();
			else  
				desPainter.GetComponent<Text> ().text = "";


		
				

		


			//desPainting.GetComponent<Text> ().text = infPaint.ToString();
			headingPaint.GetComponent<Text> ().text = "\""+namePainting [i]+"\"";
			headingPainter.GetComponent<Text> ().text = "Painted by \n"+namePainter [i];
			//desPainter.GetComponent<Text> ().text = infAuth.ToString();


			headingPaint.transform.SetParent (element.transform, false);
			desPainting.transform.SetParent(element.transform, false);
			headingPainter.transform.SetParent(element.transform, false);
			desPainter.transform.SetParent(element.transform, false);
		}
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
