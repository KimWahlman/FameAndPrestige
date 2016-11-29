using UnityEngine;
using System.Collections;

public class DuelAction : Actions{


    public int cost = 2;
    int earnedPoints = 8;

    public override void useAction(string theme)
    {
        Debug.Log("action DUEL has been triggered");
        Debug.Log("theme is " + theme);


        //networkManager.
    }
}
