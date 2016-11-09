using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    string nickname;
    public int idPlayer;
    bool canPlay = false;
    public Dictionary<int, Card> cardsHeld  = new Dictionary<int, Card>();
    public Dictionary<int, Player> opponents = new Dictionary<int, Player>();
   
}
