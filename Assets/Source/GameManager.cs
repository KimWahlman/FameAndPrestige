using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Player myPlayer;
    public Card[] Deck;
    public List<int> toPlay;
    public Hands[] playerHands = new Hands[4];
    public GameObject[] handsGO;
    public bool debugMode;
    public PlayableZone playableZone;
    public Button EndTurnBt;
    public Button PlayCardsBt;
    public Image currentImage;
    public Sprite[] Sprites;
    private NetworkManager networkManager;
	private LoadCards loadCards;
    public Dictionary<string, int> pointsDictionnary;
    
	void Start()
    {

		loadCards = GameObject.Find("LoadCards").GetComponent<LoadCards>();
		loadCards.Load ();

        if(debugMode)
        {
            initGame(0);
            StartCoroutine("firstDrawToEveryone", 0);
        }
        currentImage.sprite = Sprites[0];
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        pointsDictionnary = new Dictionary<string, int>();
        pointsDictionnary["0"] = 0;
        pointsDictionnary["1"] = 0;
        pointsDictionnary["2"] = 0;
        pointsDictionnary["3"] = 0;

		UpdatePoints ();
    }

	public void UpdatePoints(){
		GameObject cv = GameObject.Find ("Canvas");
		cv.GetComponentsInChildren<Text>()[0].text = "Player1: " + pointsDictionnary["0"].ToString() + " Player2: " + pointsDictionnary["1"].ToString() + " Player3: " + pointsDictionnary["2"].ToString() + " Player4: " + pointsDictionnary["3"].ToString();
	}

	public void CheckWinner(){
		if (pointsDictionnary ["0"] >= 10) {
		}
		if (pointsDictionnary ["1"] >= 10) {
		}
		if (pointsDictionnary ["2"] >= 10) {
		}
		if (pointsDictionnary ["3"] >= 10) {
		}

	}

    public void initGame(int playerID)
    {
        Debug.Log("game initialization");
        //assign the playerID 0 to 3
        myPlayer.idPlayer = playerID;

        //assign the hand position relative to the gameobject    
        for (int i = 0; i < 4; i++)
        {
            playerHands[playerID] = handsGO[i].GetComponent<Hands>();

            //assign the opponents to the player
            if(playerID != myPlayer.idPlayer)
            {
                GameObject tmpGO = new GameObject();
                tmpGO.AddComponent<Player>();
                myPlayer.opponents.Add(playerID, tmpGO.GetComponent<Player>());
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
                rdmCard = Random.Range(0, Deck.Length-1);
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
        foreach (var sprite in Sprites)
            if (sprite.name == theme)
                currentImage.sprite = sprite;
    }

    public bool checkPlayerTurn()
    {
        return myPlayer.canPlay;
    }
    
    public void endTurn()
    {
        myPlayer.canPlay = false;
        EndTurnBt.interactable = false;
        PlayCardsBt.interactable = false;

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
        EndTurnBt.interactable = true;
        PlayCardsBt.interactable = true;
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
            Debug.Log("card held : " + card.Value);
            card.Value.PositionOnTheHand(playerHands[myPlayer.idPlayer].posList[id++]);
        }

        
    }

    //should be called when the cards are played
    public void ReOrderPlayerHandAfterPlay(int playerID, List<int> cardIDs)
    {
        print(" in reorderhand ");

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

        foreach(var c in cardIDs)
            playerHands[playerID].deadCard();
    }

    public void storeCard(int cardID)
    {
        toPlay.Add(cardID);
		playableZone.addCard ();
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
    }

    public void playCard(int cardID)
    {
        Deck[cardID].playCard();
        
        if(!Deck[cardID].isMine)
        {
            Deck[cardID].PositionOnTheHand(playableZone.getSlot());
            playableZone.addCard();
        }
    }
    
    public void InvalidCardPlayed(string[] cardIDs)
    {
        foreach (var c in cardIDs)
        {
            var cc = int.Parse(c.Trim(new System.Char[] { ' ', '"', ',', '[', ']' }));

            myPlayer.cardsHeld.Add(cc, Deck[cc]);

            //get the next position available in the player hand
            Deck[cc].handPosition = playerHands[myPlayer.idPlayer].newCard().position;
            Deck[cc].returnBackToHand();
            removeStoredCard(cc);
        }
       
        playableZone.emptyZone();
        
    }

    void opponentdrawCard(int cardID)
    {
       
    }

    void discardCard(int cardID)
    {
        myPlayer.cardsHeld.Remove(cardID);
    }
}
