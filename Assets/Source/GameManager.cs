using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Player myPlayer;
    public Card[] Deck;
    private Hands[] playerHands = new Hands[4];
    public GameObject[] handsGO;

    void Start()
    {
        initGame(0);
        StartCoroutine("firstDrawToEveryone", 0);
    }

    void initGame(int playerID)
    {
        //assign the playerID 0 to 3
        Player.idPlayer = playerID;

        //assign the hand position relative to the gameobject    
        for (int i = 0; i < 4; i++)
        {
            playerHands[playerID] = handsGO[i].GetComponent<Hands>();

            if (playerID > 2)
                playerID = 0;
            else
                playerID++;
        }
    }

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
    
    void endTurn() {}

    void startTurn() {}
 
    public void drawCard(int cardID, int playerID)
    {
        if(playerID == Player.idPlayer)
        {
            //add the card in the player hand
            myPlayer.cardsHeld.Add(cardID, Deck[cardID]);

            //spawn the card, add it the the hand array, set the owner id
            Deck[cardID].popCard(true, playerHands[playerID].newCard(), playerID);
        }
        else
        {
            Deck[cardID].popCard(false, playerHands[playerID].newCard(), playerID);
        }
    }

    //should be called when a card is played/discarded
    public void ReOrderPlayerHand(int cardID, int playerID)
    {
        if(playerID == Player.idPlayer)
        {
            myPlayer.cardsHeld.Remove(cardID);
            playerHands[playerID].deadCard();

            int id = 0;
            foreach (KeyValuePair<int, Card> entry in myPlayer.cardsHeld)
            {
                entry.Value.AssignNewPosition(playerHands[playerID].posList[id++]);
            }
        }

    }
    

    void opponentdrawCard(int cardID)
    {
       
    }

    void playCard(int cardID)
    {
        if(myPlayer.cardsHeld[cardID] != null)
            myPlayer.cardsHeld[cardID].useCard();
    }

    void discardCard(int cardID)
    {
        myPlayer.cardsHeld.Remove(cardID);
    }
}
