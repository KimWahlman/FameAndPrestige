using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public Player myPlayer;
    public Card[] Deck;
    public List<int> toPlay; //cards to play before clicking on play cards
    public Hands[] playerHands = new Hands[4];
    public GameObject[] handsGO;
	public Animator themeBar;
    public PlayableZone playableZone;
    private NetworkManager networkManager;
	private LoadCards loadCards;
	public int TotalTurn  = 32;
    public Dictionary<string, int> pointsDictionnary;

    public bool debugMode;

    //UI
    public UIManager UIManager;
    public Text currentText;
    public Sprite[] SpritesTheme;
    public Sprite[] SpriteCharacters;
    public SpriteRenderer[] RendererCharacters;
	public Text WinText;
	public Image WinImage;
	public Text Turns;

    public Text[] scoreText;
    public Dictionary<int, Text> scoreDictionnary = new Dictionary<int, Text>();
    public Text[] inkText;
    public Dictionary<int, Text> inkDictionnary = new Dictionary<int, Text>();

    void Start()
    {

		loadCards = GameObject.Find("LoadManager").GetComponent<LoadCards>();
		loadCards.Load ();

        if(debugMode)
        {
            initGame(0);
            StartCoroutine("firstDrawToEveryone", 0);
        }
		currentText.text = SpritesTheme[0].name;
		//playableZone.GetComponent<SpriteRenderer> ().sprite =  SpritesTheme[0];
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		Turns.text = "8";
    }

	public void UpdatePoints(int PlayerID, int Score){

        if(myPlayer.idPlayer == PlayerID)
        {
            myPlayer.Score = Score;
			scoreDictionnary [PlayerID].text = "";
            scoreDictionnary[PlayerID].text = "Score : " + myPlayer.Score;
        }
        else
        {
            myPlayer.opponents[PlayerID].Score = Score;
			scoreDictionnary[PlayerID].text = "";
            scoreDictionnary[PlayerID].text = "Score : " + myPlayer.opponents[PlayerID].Score;
        }
    }

    public void UpdateInk(int PlayerID, int totInk)
    {
        if (myPlayer.idPlayer == PlayerID)
        {
            myPlayer.Ink = totInk;
            inkDictionnary[PlayerID].text = "Ink : " + myPlayer.Ink + "/16";
        }
        else
        {
            myPlayer.opponents[PlayerID].Ink = totInk;
            inkDictionnary[PlayerID].text = "Ink : " + myPlayer.opponents[PlayerID].Ink + "/16";
        }
    }

	public void CheckWinner(string string_id){

		int id = int.Parse(string_id.Trim(new System.Char[] { ' ', '"', ',', '[', ']' }));
		if (id == myPlayer.idPlayer) {
			Debug.Log ("------I WIN-------");
			WinText.text = "You Won";

		} else {
			Debug.Log ("---------I LOST---------");
			id += 1;
			WinText.text = "Player " + id.ToString() + " won!" ;
		}
		WinText.gameObject.SetActive (true);
		WinImage.gameObject.SetActive (true);

	}
		
	public void checkEndTurnButton(){
	
		if (playableZone.nextEmptySlot != 0) {
            UIManager.EndTurnBt.interactable = false;
		} else {
            UIManager.EndTurnBt.interactable = true;
		}
	}

    public void initGame(int playerID)
    {
        //assign the playerID 0 to 3
        myPlayer.idPlayer = playerID;

        //assign the hand position relative to the gameobject    
        for (int i = 0; i < 4; i++)
        {
            playerHands[playerID] = handsGO[i].GetComponent<Hands>();
            scoreDictionnary.Add(playerID, scoreText[i]);
            inkDictionnary.Add(playerID, inkText[i]);

            //assign the opponents to the player
            if (playerID != myPlayer.idPlayer)
            {
                GameObject tmpGO = new GameObject();
                tmpGO.AddComponent<Player>();
                myPlayer.opponents.Add(playerID, tmpGO.GetComponent<Player>());
                Destroy(tmpGO);
            }
               
            if (playerID > 2)
                playerID = 0;
            else
                playerID++;
        }
    }

    //server side
    IEnumerator firstDrawToEveryone(int drawerID)
    {
        for(int amountOfCards = 0; amountOfCards < 16; ++amountOfCards)
        {
            int rdmCard;
            do
            {
                rdmCard = UnityEngine.Random.Range(0, Deck.Length-1);
            }
            while (Deck[rdmCard].gameObject.activeSelf);

            drawCard(rdmCard, drawerID++);
            
            if (drawerID > 3)
                drawerID = 0;

            yield return new WaitForSeconds(.1f);
        }
    }

    public void ChangeTheme(string theme)
    {
		int index = 0;
		foreach (var sprite in SpritesTheme) {
			if (sprite.name == theme) {
				currentText.text = sprite.name;
				//playableZone.GetComponent<SpriteRenderer> ().sprite = sprite;
				break;
			}
			index++;
		}
		Debug.Log (index);
		themeBar.SetInteger ("theme", index);
    }

    public bool checkPlayerTurn()
    {
        return myPlayer.canPlay;
    }
    
    public void endTurn()
    {
        myPlayer.canPlay = false;
        UIManager.EndTurnBt.interactable = false;
        UIManager.PlayCardsBt.interactable = false;
        UIManager.ActionBt.interactable = false;

    }

    public void cleanBoard()
    {
        playableZone.emptyZone();
        foreach(var c in Deck)
        {
            if (c.hasBeenPlayed)
                c.hideCard();
        }
        foreach(var c in toPlay)
        {
            Deck[c].returnBackToHand();
        }
    }
    public void startTurn()
    {
        myPlayer.canPlay = true;
        UIManager.EndTurnBt.interactable = true;
        UIManager.PlayCardsBt.interactable = true;
        UIManager.ActionBt.interactable = true;
    }

    public void AssignCharacters(List<String> characters)
    {
        for(int i = 0; i < 4; ++i)
        {
            if(i == myPlayer.idPlayer)
            {
                myPlayer.character = new Character();
                myPlayer.character.selectChar(characters[i]);
            } else
            {
                myPlayer.opponents[i].character = new Character();
                myPlayer.opponents[i].character.selectChar(characters[i]);
            }
        }

        var id = myPlayer.idPlayer;
        foreach(SpriteRenderer charRend in RendererCharacters)
        {
            if (id == myPlayer.idPlayer)
            {
                foreach (Sprite charSprite in SpriteCharacters)
                    if (charSprite.name == myPlayer.character.assignedCharacter.ToString())
                        charRend.sprite = charSprite;
            } else
            {
                foreach (Sprite charSprite in SpriteCharacters)
                    if (charSprite.name == myPlayer.opponents[id].character.assignedCharacter.ToString())
                        charRend.sprite = charSprite;
            }

            id++;
            if (id > 3)
                id = 0;
        }
    }

    //server side
    public void drawCard(int cardID, int playerID)
    {
        if(playerID == myPlayer.idPlayer)
        {
            print("DRAW CARD : player iD : " + playerID + " card id :  " + cardID);
            //add the card in the player hand
            myPlayer.cardsHeld.Add(cardID, Deck[cardID]);

            //spawn the card, add it the the hand array, set the owner id
            Deck[cardID].popCard(true, playerHands[playerID].newCard(), playerID);
        }
        else
        {
            print("DRAW CARD : player iD : " + playerID + " card id :  " + cardID);
            myPlayer.opponents[playerID].cardsHeld.Add(cardID, Deck[cardID]);

            Deck[cardID].popCard(false, playerHands[playerID].newCard(), playerID);
        }
    }
    
    public void ReOrderPlayerHandAfterDrop(int cardID)
    {
        //Card is removed from player 
        myPlayer.cardsHeld.Remove(cardID);
        playerHands[myPlayer.idPlayer].deadCard();      

        int id = 0;
        foreach (KeyValuePair<int, Card> card in myPlayer.cardsHeld)
        {
            card.Value.PositionOnTheHand(playerHands[myPlayer.idPlayer].posList[id++]);
        }
    }

    public void ReOrderPlayerHandAfterReturn(int cardID,Card draggedCard)
    {        
        //Card is back to player 
        myPlayer.cardsHeld.Add(cardID, draggedCard);
        draggedCard.handPosition = playerHands[myPlayer.idPlayer].newCard().position;
        draggedCard.gameObject.layer = 1;
        Debug.Log("draggedcard position::"+ draggedCard.handPosition);    

        int id = 0;
        foreach (KeyValuePair<int, Card> card in myPlayer.cardsHeld)
        {
            Debug.Log("card held : " + card.Value);
            card.Value.PositionOnTheHand(playerHands[myPlayer.idPlayer].posList[id++]);
        }

        draggedCard.isOnTheBoard = false;
    }

    //should be called when the cards are played
    public void ReOrderPlayerHands(int playerID, List<int> cardIDs)
    {
        print(" in reorderhand , id player  == " + playerID);
        
        foreach (var c in cardIDs)
            playerHands[playerID].deadCard();

        if (playerID == myPlayer.idPlayer)
        {
            foreach (int cardID in cardIDs)
                myPlayer.cardsHeld.Remove(cardID);

            int id = 0;
            foreach (KeyValuePair<int, Card> card in myPlayer.cardsHeld)
            {
                card.Value.PositionOnTheHand(playerHands[playerID].posList[id++]);
            }
        }
        else
        {
            foreach (int cardID in cardIDs)
                myPlayer.opponents[playerID].cardsHeld.Remove(cardID);

            int id = 0;
            foreach (KeyValuePair<int, Card> card in myPlayer.opponents[playerID].cardsHeld)
            {
                card.Value.PositionOnTheHand(playerHands[playerID].posList[id++]);
            }
        }

    }

    public void storeCard(int cardID)
    {
        toPlay.Add(cardID);
		playableZone.addCard ();
    }

    public bool checkStoredCard(int cardID)
    {
        return toPlay.IndexOf(cardID) != -1;
    }

    public void removeStoredCard(int cardID)
    {
        if(toPlay.Contains(cardID))
        {
            toPlay.Remove(cardID);
            playableZone.removeCard();
            reOrderPlayingZone();
        }
    }

    public void reOrderPlayingZone()
    {
        playableZone.emptyZone();
        foreach ( var c in toPlay)
        {
            Deck[c].PositionOnTheBoard(playableZone.getSlot());
            playableZone.addCard();
        }
    }

    public void sendStoredCards()
    {
        if(toPlay.Count != 0)
            networkManager.SendPlayCard(myPlayer.idPlayer, toPlay);

        UIManager.EndTurnBt.interactable = true;
    }

    public void playCard(int cardID)
    {
        Deck[cardID].playCard();
        
        if(!Deck[cardID].isMine)
        {
            Deck[cardID].PositionOnTheHand(playableZone.getSlot());
			Deck [cardID].title.gameObject.SetActive (true);
			Deck [cardID].description.gameObject.SetActive (true);
			Deck [cardID].cardArt.SetActive (true);
			Deck [cardID].point.gameObject.SetActive (true);
			Deck [cardID].isOnTheBoard = true;
            playableZone.addCard();
        }
    }
    
    public void InvalidCardPlayed(string[] cardIDs)
    {        
        foreach (var c in cardIDs)
        {
            var cc = int.Parse(c.Trim(new System.Char[] { ' ', '"', ',', '[', ']' }));
            myPlayer.cardsHeld.Add(cc, Deck[cc]);
            removeStoredCard(cc);
        }

        playerHands[myPlayer.idPlayer].emptyHand();

        foreach (KeyValuePair<int, Card> card in myPlayer.cardsHeld)
        {
            Debug.Log("card to return : " + card.Value.id);
            card.Value.handPosition = playerHands[myPlayer.idPlayer].newCard().position;
            card.Value.returnBackToHand();
        }
        
        playableZone.emptyZone();
        
    }

    void opponentdrawCard(int cardID)
    {
       
    }

    public void discardCard(int cardID, int playerID)
    {
        Deck[cardID].hasBeenPlayed = true;
        Deck[cardID].hideCard();
        List<int> cardIDtoRemove = new List<int>();
        cardIDtoRemove.Add(cardID);
        ReOrderPlayerHands(playerID, cardIDtoRemove);
    }

    public void usePower()
    {
        myPlayer.character.usePower();
    }
}
