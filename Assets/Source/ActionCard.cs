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

    public void Shame() { }
    public void GhostWriting() {}
    public void Duel() { }
}
