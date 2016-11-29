using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShameAction : Actions {


    int cost = 2;
    int earnedPoints = 5;

    public override void useAction(string theme)
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Debug.Log("action SHAME has been triggered");
        Debug.Log("theme is " + theme);

        networkManager.SendShame(gameManager.myPlayer.idPlayer, SelectedPlayerId, theme, cost, earnedPoints);
        gameManager.UIManager.ActionBt.interactable = false;
    }

    public int getCost()
    {
        return cost;
    }

}
