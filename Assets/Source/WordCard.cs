using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordCard : Card {

    public int value;
    public cardTheme[] MyCardThemes;

    public enum cardTheme
    {
        HORROR = 0,
        FOLKLORE = 1,
        NATURE = 2,
        HISTORY = 3
    };

    public override void useCard()
    { }
}