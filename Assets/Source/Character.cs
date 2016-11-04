using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    
    public character MyCharacter;

    public enum character
    {
        MARY_SHELLEY = 0,
        THE_GRIM_BROTHER = 1,
        WILLIAM_WORDSWORTH = 2,
        BETTINA_VON_ARMIN = 3
    };

    public void usePower()
    {
        switch (MyCharacter)
        {
            case character.MARY_SHELLEY:
                Power_Mary();
                break;
            case character.THE_GRIM_BROTHER:
                Power_GrimBrother();
                break;
            case character.WILLIAM_WORDSWORTH:
                Power_William();
                break;
            case character.BETTINA_VON_ARMIN:
                Power_Bettina();
                break;
        }
    }

    public void Power_Mary() { }
    public void Power_GrimBrother() { }
    public void Power_William() { }
    public void Power_Bettina() { }

}
