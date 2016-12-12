using UnityEngine;
using System.Collections;

public class Character {
    
    public character assignedCharacter;
    public NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    public string name;

    public enum character
    {
        MARY_SHELLEY = 0,
        THE_GRIM_BROTHERS = 1,
        WILLIAM_WORDSWORTH = 2,
        BETTINA_VON_ARMIN = 3
    };

    public void selectChar(string charName)
    {
        character c = (character)character.Parse(typeof(character), charName, true);
        assignedCharacter = c;
        chooseName();

    }
    public void chooseName()
    {
        switch (assignedCharacter)
        {
            case character.MARY_SHELLEY:
                name = "Mary Shelley";
                break;
            case character.THE_GRIM_BROTHERS:
                name = "The Grim Brothers";
                break;
            case character.WILLIAM_WORDSWORTH:
                name = "William Wordsworth";
                break;
            case character.BETTINA_VON_ARMIN:
                name = "Bettina Von Armin";
                break;
        }
    }

    public void usePower()
    {
        Debug.Log("POWER USED " + assignedCharacter);
        switch (assignedCharacter)
        {
            case character.MARY_SHELLEY:
                Power_Mary();
                break;
            case character.THE_GRIM_BROTHERS:
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

    public void Power_Mary()
    {
        networkManager.SendPower("MARY");
    }
    public void Power_GrimBrother()
    {
        networkManager.SendPower("MARY");
    }
    public void Power_William()
    {
        networkManager.SendPower("MARY");
    }
    public void Power_Bettina()
    {
        networkManager.SendPower("MARY");
    }

}
