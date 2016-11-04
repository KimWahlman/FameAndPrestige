using UnityEngine;
using System.Collections;

public abstract class Card : MonoBehaviour
{

    public int id;
    public string cardName;
    public int cost;
    public bool hasBeenPlayed;

    abstract public void useCard();

}