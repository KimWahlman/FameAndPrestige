using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour {
   
    public GameManager gameManager;

    public int debugcardID;
    public int debugplayerID;

    public string msg;

    void messageReceived(string message)
    {
        if (message != "")
        {
            string[] msg = message.Split('|');
            
            if (msg[0] != "")
            {
                switch (msg[0])
                {
                    case "DRAW":
                        // 1 player; 2 card
                        gameManager.drawCard(Convert.ToInt32(msg[1]), Convert.ToInt32(msg[2]));
                        break;

                    case "PLAYCARD":
                        // 1 player ; 2 card
                        gameManager.playCard(Convert.ToInt32(msg[2]));
                        gameManager.ReOrderPlayerHand(Convert.ToInt32(msg[1]), Convert.ToInt32(msg[2]));
                        break;                        
                }
            }
        }
    }


    public static void sendMessage(string message)
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            messageReceived(msg);
    }



}
