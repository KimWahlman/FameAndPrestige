using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hands : MonoBehaviour {

    public List<Transform> posList = new List<Transform>();
    int playerID;
    public int amountOfCards = 0;
    public TextMesh textPlayerID;

    public Transform newCard()
    {
        return posList[amountOfCards++];
    }

    public void deadCard()
    {
        amountOfCards--;
    }
}
