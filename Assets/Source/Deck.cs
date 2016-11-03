using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    public GameObject prefab;
    public bool mouseOver = false;
    public ArrayList deck = new ArrayList();
    public int currentNumOfCardsInDeck;
    public int[] theDeck = new int[50];
    public string[] msg = new string[3]; // example ["DRAW", "1", "3"] // Action, somthing, amount of cards to draw
    /*
     * TODO: Make the drawn card random
     * Hold an array that keep track of how many 
     * of a specific card is still in the deck.
     
    
    void HandleMessage()
    {
        switch (msg[0])
        {
            case "DRAW":
                DrawCard()
                break;
            default:
                break;
        }
    }

    */
    // Use this for initialization
    void Start ()
    {
        currentNumOfCardsInDeck = 50;
        for (int i = 0; i < currentNumOfCardsInDeck; i++)
        {
            int temp = Random.Range(1, 3);
            deck.Add(temp);
            //theDeck
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(mouseOver)
            CreateNewCard();
	}

    void CreateNewCard()
    {
        if(Input.GetMouseButtonDown(0) && deck.Count > 0)
        {
            Debug.Log("1. deck size before: " + deck.Count + " deck size after: " + deck.Count);
            Instantiate(prefab, new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
            Debug.Log("2. deck size before: " + deck.Count + " deck size after: " + deck.Count);
            prefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/Card" + deck[deck.Count - 1], typeof(Sprite)) as Sprite;
            Debug.Log("3. deck size before: " + deck.Count + " deck size after: " + deck.Count);
            deck.RemoveAt(deck.Count - 1);
            Debug.Log("4. deck size before: " + deck.Count + " deck size after: " + deck.Count);
        }
        else if (deck.Count == 0)
            Debug.Log("We are out of cards!");
    }

    void OnMouseOver() { if (!mouseOver) mouseOver = true; }
    void OnMouseExit() { if (mouseOver) mouseOver = false; }
}
