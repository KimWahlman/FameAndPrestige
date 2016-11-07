using UnityEngine;
using System.Collections;

public class ActionCard : Card {

    public cardAction MyCardAction;

    public enum cardAction
    {
        SHAME = 0,
        GHOST = 1,
        DUEL = 2,
        CHANGETHEME = 3
    };
    
    public override void useCard()
    {
        switch(MyCardAction)
        {
            case cardAction.SHAME:
                Shame();
                break;
            case cardAction.GHOST:
                GhostWriting();
                break;
            case cardAction.DUEL:
                Duel();
                break;
        }       
    }

    public void Shame() { print("Action - Shame card has been played"); }
    public void GhostWriting() { print("Action - GhostWriting card has been played"); }
    public void Duel() { print("Action - Duel card has been played"); }
}