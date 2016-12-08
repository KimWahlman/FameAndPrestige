using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CardBook : MonoBehaviour {
	public GameObject deckList;
	public GameObject informationList;


	public GameObject cardElement;
	public GameObject h1Preb;
	public GameObject h2Preb;
	public GameObject h3Preb;
	public GameObject parPreb;


	private string url = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=";
	private string[] titles;
	private string[] author;
	private int count =0;

	// Use this for initialization
	void Start () {
		/*
		goback.onClick.AddListener (testOnClick);
		showCitation.onClick.AddListener (testOnClick);
		showPainting.onClick.AddListener (testOnClick);
		*/
		loadCardList ();
		//StartCoroutine( requestInformation ("Romanticism"));
	}
		

	void loadCardList(){
		
		if (count < 1) {
			count++;
			string json = Resources.LoadAll ("card_text") [0].ToString ();
			JSONObject cards = new JSONObject (json);

	
			foreach (JSONObject card_json in cards.list) {
			
				GameObject card = (GameObject)Instantiate (cardElement, new Vector3 (0, 0, 0), Quaternion.identity);


				string title = card_json ["name"].ToString ().Trim (new Char[] { ' ', '"' });
				string description = card_json ["description"].ToString ().Trim (new Char[] { ' ', '"', '\n' });
				string path = card_json ["img"].ToString ().Trim (new Char[] { ' ', '"' });
				string point = card_json ["point"].ToString ().Trim (new Char[] { ' ', '"' });
				string type = card_json ["type"].ToString ().Trim (new Char[] { ' ', '"' });
				string tmp;
				//Debug.Log (card_json);
				if (type == "pure") {
			
					titles = new string[1];
					author = new string[1];
					tmp = card_json ["title"].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					titles [0] = tmp;

					tmp = card_json ["author"].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					author [0] = tmp;
			
				} else {
				
					titles = new string[2];
					author = new string[2];
					tmp = card_json ["title"] [0].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					titles [0] = tmp;
					
					tmp = card_json ["title"] [1].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					titles [1] = tmp;


					tmp = card_json ["author"] [0].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					author [0] = tmp;
					tmp = card_json ["author"] [1].ToString ().Trim (new Char[] { ' ', '"' });
					if (tmp == "")
						tmp = "Unknow";
					author [1] = tmp;

				}


				//Debug.Log (titleob);



				Texture[] testTexture = Resources.LoadAll<Texture> ("picture/" + path);
				card.name = "Card_" + card_json ["id"].ToString ();

				card.transform.Find ("Title").GetComponent<Text> ().text = title;
				card.transform.Find ("Description").GetComponent<Text> ().text = description;
				card.transform.Find ("painting").GetComponent<RawImage> ().texture = testTexture [0];
				card.GetComponent<ShowInformation> ().informationList = informationList;
				card.GetComponent<ShowInformation> ().authoPainting = author;
				card.GetComponent<ShowInformation> ().namePainting = titles;
				card.GetComponent<ShowInformation> ().name = card.name;
				card.GetComponent<ShowInformation> ().titleCard = title;
				card.GetComponent<ShowInformation> ().cardButton = card.GetComponent<Button> ();
				//card.GetComponent<ShowInformation> ().type = type;




				card.transform.SetParent (deckList.GetComponentInChildren<ScrollRect> ().content, false);

			}

		
		}
	}


}
