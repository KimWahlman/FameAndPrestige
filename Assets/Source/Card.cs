using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

    public int cardID;
    public string name;
    public int cost;
    public string description;

    public CardType type = new CardType();


    /*  void Start()
      {
          type.TypeName = "Word";
      }

      void Update()
      {
          if (Input.GetKeyDown(KeyCode.A))
          {
              Debug.Log(type.TypeName);
          }
      }*/


}
