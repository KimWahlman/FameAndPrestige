using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using System;

public class LoadCards : MonoBehaviour {

	public GameObject cardPrefab;
	public GameObject Parent;
	private Card showCard;
	public GameManager gameManager;

	public void Load(){
		List<Card> tempDeck = new List<Card> ();

		string json = Resources.LoadAll("card_text")[0].ToString();
		JSONObject cards = new JSONObject(json);
		//Debug.Log (size);
		foreach(JSONObject card_json in cards.list){
			//Debug.Log (card_json["id"]);
			GameObject card = (GameObject)Instantiate (cardPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			card.transform.SetParent(Parent.transform);
			card.name = "Card_"+card_json ["id"].ToString();


			showCard = card.GetComponent<Card> ();
			//Debug.Log (showCard);

			string title = card_json ["name"].ToString().Trim(new Char[] { ' ', '"' });
			string description = card_json ["description"].ToString().Trim(new Char[] { ' ', '"', '\n' });
			string path = card_json ["img"].ToString().Trim(new Char[] { ' ', '"' });

			showCard.LoadResource (title, description, path);
			card.SetActive (false);

			Card cardScript = card.GetComponents<Card> ()[0];


			int cardID;
			int.TryParse(card_json ["id"].ToString(), out cardID);

			cardScript.id = cardID;
			string toolTip = "";
			foreach (JSONObject theme in card_json["theme"].list) {
				toolTip += theme.ToString().Trim(new Char[] { ' ', '"' }) +" ";
			}
			toolTip += "\n"+card_json ["point"].ToString().Trim(new Char[] { ' ', '"' });

			cardScript.toolTipText = toolTip;
			tempDeck.Add (cardScript);

		}

		gameManager.Deck = tempDeck.ToArray ();
	}
}
