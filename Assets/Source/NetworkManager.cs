using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class NetworkManager : MonoBehaviour {
   
    public GameManager gameManager;
    public SocketIOComponent socket;

    public int debugcardID;
    public int debugplayerID;

    public Player myPlayer;

    public string msg;

    void Start()
    {
        StartCoroutine("ConnectToServer");


        /*socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("PLAY", OnUserPlay);
        socket.On("USER_DISCONNECTED", OnUserDisconnected);
        socket.On("ROLL_DICE", OnRollDice);
        socket.On("SEND_CARDS", OnReceiveCards);
        socket.On("CHECK_CARD", OnReceiveCheck);
        socket.On("ROOMS", OnReceiveRooms);*/

        socket.On("ASSIGN_ID", OnReceiveAssignID);
        socket.On("INIT_GAME", OnReceiveInitGame);
        socket.On("DRAW_CARD", OnReceiveDrawCard);
    }

    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = "UNITY";
        JSONObject jso = new JSONObject(data);
        socket.Emit("USER_CONNECT", jso);

        yield return new WaitForSeconds(1f);
    }

    public void OnReceiveAssignID(SocketIOEvent e)
    {
        myPlayer.idPlayer = int.Parse(e.data["id"].ToString());
    }

    public void OnReceiveInitGame(SocketIOEvent e)
    {
        gameManager.initGame(myPlayer.idPlayer);
    }

    public void OnReceiveDrawCard(SocketIOEvent e)
    {
        print(e.data);
        gameManager.drawCard(int.Parse(e.data["id"].ToString()), int.Parse(e.data["playerId"].ToString()));
    }

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
