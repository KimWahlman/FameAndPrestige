using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ShowCard : MonoBehaviour {

    public TextMesh title;
    public TextMesh description;
	public GameObject image;

    public GameObject card;

	// Use this for initialization
	void Start () {
  
        
		string text = System.IO.File.ReadAllText("Assets/card_text.json");
		JSONObject all = new JSONObject (text);
		foreach (JSONObject j in all.list) {
			title.text = j["name"].ToString();  
			description.text = j["theme"].ToString() + "\n" + j["point"].ToString();
			//image.GetComponents<Sprite>()
			break;
		}
		/*
		string[] jsonObjects = text.Split('}');
		foreach (var s in jsonObjects) {
			string obj = s.Substring(1);
			obj = obj + "}";
			Debug.Log (obj);
			JSONObject j = new JSONObject (s);
			Debug.Log (j["name"]);
		}
		*/
    }

   
	
	// Update is called once per frame
	void Update () {
	
	}



}
