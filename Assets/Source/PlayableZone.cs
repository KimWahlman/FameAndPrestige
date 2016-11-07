using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayableZone : MonoBehaviour {

    public List<Transform> slotList = new List<Transform>();
    int nextEmptySlot = 0;

    public Vector3 getSlot()
    {
        return slotList[nextEmptySlot].position;
    }

    public void addCard()
    {
        nextEmptySlot++;
    }

    public void emptyZone()
    {
        nextEmptySlot = 0;
    }

}
