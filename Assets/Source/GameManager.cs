using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Player myPlayer;
    public Card[] Deck;
    private Hands[] playerHands = new Hands[4];
    public GameObject[] handsGO;

    void initGame(int playerID)
    {
        //assign the playerID 0 to 3
        myPlayer.idPlayer = playerID;

        //assign the hand position relative to the gameobject    
        for (int i = 0; i < 3; i++)
        {
            playerHands[playerID] = handsGO[i].GetComponent<Hands>();

            if (playerID > 3)
                playerID = 0;
            else
                playerID++;
        }
    }

    void Start()
    {
        initGame(1);
    }

    void endTurn() {}

    void startTurn() {}
 
    public void drawCard(int cardID, int playerID)
    {
        if(playerID == myPlayer.idPlayer)
        {
            //add the card in the player hand
            myPlayer.cardsHeld.Add(cardID, Deck[cardID]);

            //spawn the card
            Deck[cardID].popCard(true, playerHands[playerID].newCard());

            Deck[cardID].ownerID = playerID;

        }
        else
        {
            Deck[cardID].popCard(false, playerHands[playerID].newCard());
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
