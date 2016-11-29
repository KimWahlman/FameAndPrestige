using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Actions : MonoBehaviour {


    public NetworkManager networkManager;
    public GameManager gameManager;
    public UIManager UIManager;

    public static int SelectedPlayerId;
    
    public actionEnum selectedAction;

    public enum actionEnum
    {
        SHAME = 0,
        DUEL = 1
    };

    public virtual void useAction(string theme)
    {
    }

    public void SelectPlayer(int buttonPosition)
    {
        SelectedPlayerId = gameManager.myPlayer.idPlayer;

        for (int i = 0; i < buttonPosition; ++i)
        {
            SelectedPlayerId++;

            if (SelectedPlayerId > 3)
            {
                SelectedPlayerId = 0;
            }
        }

        print("Player ID : " + SelectedPlayerId);
    }

    public void SelectAction(int action)
    {
        switch(action)
        {
            case 0:
                UIManager.AssignActionToTheme(actionEnum.SHAME);
                break;
            case 1:
                UIManager.AssignActionToTheme(actionEnum.DUEL);
                break;
        }
    }
    
}
