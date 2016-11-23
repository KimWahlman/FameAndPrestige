using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Player : MonoBehaviour {

    string nickname;
    public int idPlayer;
    public bool canPlay = false;
    public Dictionary<int, Card> cardsHeld = new Dictionary<int, Card>();

   // public OrderedDictionary cardsHeld = new OrderedDictionary();

    public Dictionary<int, Player> opponents = new Dictionary<int, Player>();

    void Start()
    {

    }
   
}
