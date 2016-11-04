using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    string nickname;
    int idPlayer;
	int score;
    bool canPlay = false;
    public Dictionary<int, Card> cardsHeld  = new Dictionary<int, Card>();
    
}
