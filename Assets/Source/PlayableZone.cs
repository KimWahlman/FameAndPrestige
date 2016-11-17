using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayableZone : MonoBehaviour {

    public List<Transform> slotList = new List<Transform>();
    int nextEmptySlot = 0;

    public Transform getSlot()
    {
        return slotList[nextEmptySlot];
    }

    public void addCard()
    {
        nextEmptySlot++;
    }

    public void removeCard()
    {
        if(nextEmptySlot != 0)
            nextEmptySlot--;

    }

    public void emptyZone()
    {
        nextEmptySlot = 0;
    }

}
