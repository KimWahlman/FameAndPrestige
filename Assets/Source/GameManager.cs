using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Player myPlayer;
    public Card[] Deck;
    private Hands[] playerHands = new Hands[4];
    public GameObject[] handsGO;
    public bool debugMode;
    public PlayableZone playableZone;

    public Button EndTurnBt;

    void Start()
    {
        if(debugMode)
        {
            initGame(0);
            StartCoroutine("firstDrawToEveryone", 0);
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

    public bool checkPlayerTurn()
    {
        return myPlayer.canPlay;
    }
    
    public void endTurn()
    {
        myPlayer.canPlay = false;
        EndTurnBt.interactable = false;

    }

    public void cleanBoard()
    {
        playableZone.emptyZone();
        foreach(var c in Deck)
        {
            if (c.hasBeenPlayed)
                c.hideCard();
        }
    }
    public void startTurn()
    {
        myPlayer.canPlay = true;
        EndTurnBt.interactable = true;
    }
 
    //server side
    public void drawCard(int cardID, int playerID)
    {
        if(playerID == myPlayer.idPlayer)
        {
            print("player iD : " + playerID + " card id :  " + cardID);
            //add the card in the player hand
            myPlayer.cardsHeld.Add(cardID, Deck[cardID]);

            //spawn the card, add it the the hand array, set the owner id
            Deck[cardID].popCard(true, playerHands[playerID].newCard(), playerID);
        }
        else
        {
            print("player iD : " + playerID + " card id :  " + cardID);
            myPlayer.opponents[playerID].cardsHeld.Add(cardID, Deck[cardID]);

            Deck[cardID].popCard(false, playerHands[playerID].newCard(), playerID);
        }
    }



    //should be called when a card is played/discarded
    public void ReOrderPlayerHand(int playerID, int cardID)
    {
        print(" in reorderhand ");

        if (playerID == myPlayer.idPlayer)
        {
            myPlayer.cardsHeld.Remove(cardID);

            int id = 0;
            foreach (KeyValuePair<int, Card> card in myPlayer.cardsHeld)
            {
                card.Value.PositionOnTheHand(playerHands[playerID].posList[id++]);
            }
        }
        else
        {
            myPlayer.opponents[playerID].cardsHeld.Remove(cardID);

            int id = 0;
            foreach (KeyValuePair<int, Card> card in myPlayer.opponents[playerID].cardsHeld)
            {
                card.Value.PositionOnTheHand(playerHands[playerID].posList[id++]);
            }
        }
        playerHands[playerID].deadCard();
    }

    public void playCard(int cardID)
    {
        Deck[cardID].playCard();
        
        if(!Deck[cardID].isMine)
            Deck[cardID].PositionOnTheHand(playableZone.getSlot());

        playableZone.addCard();
    }
    
    public void InvalidCardPlayed(int cardID)
    {   
        Deck[cardID].returnBackToHand();
        playableZone.removeCard();
    }

    void opponentdrawCard(int cardID)
    {
       
    }

    void discardCard(int cardID)
    {
        myPlayer.cardsHeld.Remove(cardID);
    }
}
