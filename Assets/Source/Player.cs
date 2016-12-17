using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    string nickname;
    public int idPlayer;
    public bool canPlay = false;
    public Dictionary<int, Card> cardsHeld = new Dictionary<int, Card>();
    public int Score = 0;
    public int Ink = 0;
    public Character character;
    public Dictionary<int, Player> opponents = new Dictionary<int, Player>();
    public int Tries = 0;
    public GameObject quillSprite;
    

}
