using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LastPlayZone : MonoBehaviour {

    public List<Transform> posList = new List<Transform>();
    public GameManager gameManager;
    public List<Card> currentCardsList;

    public void FillZone(List<Card> cardList)
    {
        EmptyZone();
        
        int i = 0;
 
        foreach(var c in cardList)
        {  
            Card cardCopyGo = Instantiate(c, new Vector3(0, 0, 0), Quaternion.identity) as Card;
            cardCopyGo.PositionOnTheBoard(posList[i].transform);
            cardCopyGo.transform.localScale *= 0.7f;
            cardCopyGo.isOnTheBoard = false;
            currentCardsList.Add(cardCopyGo);
            
            c.hideCard();
            i++;
        }
    }

    public void EmptyZone()
    {
        foreach(var c in currentCardsList)
        {
            c.destroyCard();
        }
        currentCardsList = new List<Card>();
    }

    public bool CheckContainCard(int cardID)
    {
        bool contain = false;

        foreach(var c in currentCardsList)
        {
            if (c.id == cardID)
                contain = true;
        }

        return contain;
    }
    
}
