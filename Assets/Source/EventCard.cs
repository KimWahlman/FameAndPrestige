using UnityEngine;
using System.Collections;

public class EventCard : Card {

    public cardEvent MyCardEvent;

    public enum cardEvent
    {
        DYNAMITE = 0,
        CHANGEMIND = 1,
        COPYCARD = 2,
        STORM = 3,
        CHANGETHEME = 4
    };
    
    public override void useCard()
    {
        switch (MyCardEvent)
        {
            case cardEvent.DYNAMITE:
                Dynamite();
                break;
            case cardEvent.CHANGEMIND:
                ChangeMind();
                break;
            case cardEvent.COPYCARD:
                CopyCard();
                break;
            case cardEvent.STORM:
                Storm();
                break;
            case cardEvent.CHANGETHEME:
                ChangeTheme();
                break;
        }
    }


    public void Dynamite() { }
    public void ChangeMind() { }
    public void CopyCard() { }
    public void Storm() { }
    public void ChangeTheme() { }

}
