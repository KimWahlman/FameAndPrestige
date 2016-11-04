using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    
    public Player myPlayer;
    public Card[] Deck;

    void endTurn() {}

    void startTurn() {}

    void drawCard(int cardID)
    {
        myPlayer.cardsHeld.Add(cardID, Deck[cardID]);
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
