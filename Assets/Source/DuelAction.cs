using UnityEngine;
using System.Collections;

public class DuelAction : Actions{


    int cost = 4;
    int earnedPoints = 8;

    public override void useAction(string theme)
    {
        Debug.Log("action DUEL has been triggered");
        Debug.Log("theme is " + theme);


        //networkManager.
    }

    public int getCost()
    {
        return cost;
    }
}
