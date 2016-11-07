using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
   
    public GameManager gameManager;

    public int debugcardID;
    public int debugplayerID;

    void messageReceived(string message)
    {
        
    }

    public static void sendMessage(string message)
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            gameManager.drawCard(debugcardID, debugplayerID);
    }



}
