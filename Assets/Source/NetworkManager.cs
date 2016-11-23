using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviour {
   
    public GameManager gameManager;
    public SocketIOComponent socket;


    public int debugcardID;
    public int debugplayerID;

    public Player myPlayer;

    public string msg;
	//private string Server = "ws://193.11.161.137:3000/socket.io/?EIO=4&transport=websocket";
	private string localServer = "ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket";


    void Start()
    {
        StartCoroutine("ConnectToServer");

        socket.url = localServer;

        socket.On("ASSIGN_ID", OnReceiveAssignID);
        socket.On("INIT_GAME", OnReceiveInitGame);
        socket.On("DRAW_CARD", OnReceiveDrawCard);
        socket.On("PLAY_CARD", OnReceivePlayCard);
        socket.On("INVALID_PLAY_CARD", OnReceiveInvalidPlayCard);
        socket.On("CHANGE_TURN", OnReceiveChangeTurn);
        socket.On("CHANGE_THEME", onChangeTheme);
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

    public void SendPlayCard(int playerID, List<int> cardIDs)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["playerID"] = playerID.ToString();
		string st = "";
		for (int i = 0; i < cardIDs.Count; i++) {
			if (i == cardIDs.Count - 1)
				st += cardIDs [i];
			else
				st += cardIDs [i] + ",";
		}
        data["cardID"] = st;       

        JSONObject jso = new JSONObject(data);
        socket.Emit("PLAY_CARD", jso);
    }

    //Submit your play cards with your button


    public void SendEndTurn()
    {
        socket.Emit("END_TURN");
    }

    public void onChangeTheme(SocketIOEvent e)
    {
		print("Change theme received " + e.data);
        string a = e.data.GetField("theme").ToString();
        string themeName = a.Trim(new Char[] {'"'});
        gameManager.ChangeTheme(themeName);
    }

    public void OnReceivePlayCard(SocketIOEvent e)
    {
        string cards = e.data.GetField("cards").ToString();
        string player = e.data.GetField("playerID").ToString();
        string totalPoints = e.data.GetField("totalPoints").ToString();

        int playerID;
        int.TryParse(player, out playerID);
        int totPoints;
        int.TryParse(totalPoints.Trim(new Char[] {'"'}), out totPoints);

		Debug.Log ("Point String " + totalPoints);
		Debug.Log ("EARN point " + totPoints);

		gameManager.pointsDictionnary [player] += totPoints;

		gameManager.UpdatePoints ();

        gameManager.toPlay = new List<int>();
        
        var splitedCardsID = cards.Split(',');
        List<int> cardsToRemoveFromHand = new List<int>();
      
        foreach ( var c in splitedCardsID )
        {
            string cc = c.Trim(new Char[] { ' ', '"', ',' });
            int cardID;
            int.TryParse(cc, out cardID);
            gameManager.playCard(cardID);

            cardsToRemoveFromHand.Add(cardID);
        }
        
        if(gameManager.myPlayer.idPlayer == playerID)
        {
            gameManager.myPlayer.canPlay = false;
            gameManager.PlayCardsBt.interactable = false;
        } else
        {
            gameManager.ReOrderPlayerHandAfterPlay(playerID, cardsToRemoveFromHand);
        }

    }

    public void OnReceiveInvalidPlayCard(SocketIOEvent e)
    {
        string player = e.data.GetField("playerID").ToString();
        int playerID;
        int.TryParse(player, out playerID);

        print("INVALID PLAY CARD RECEIVED   " + e.data.GetField("cards").ToString());

        if(myPlayer.idPlayer == playerID)
        {
            string[] cards = e.data.GetField("cards").ToString().Split(',');
            gameManager.InvalidCardPlayed(cards);
        }


        /*
        foreach (var c in cards)
        {
            var cc = c.Trim(new Char[] { ' ', '"', ',', '[', ']' });
            gameManager.InvalidCardPlayed(int.Parse(cc));
        }*/
    }

    public void OnReceiveAssignID(SocketIOEvent e)
    {
        myPlayer.idPlayer = int.Parse(e.data["id"].ToString());
    }

    public void OnReceiveInitGame(SocketIOEvent e)
    {
        gameManager.initGame(myPlayer.idPlayer);
    }

    public void OnReceiveChangeTurn(SocketIOEvent e)
    {
        int playerIdTurn = int.Parse(e.data["playerId"].ToString());

        if (myPlayer.idPlayer == playerIdTurn)
        {
            gameManager.startTurn();
        } else
        {
            gameManager.endTurn();
        }

        gameManager.cleanBoard();
    }

    public void OnReceiveDrawCard(SocketIOEvent e)
    {
        gameManager.drawCard(int.Parse(e.data["id"].ToString()), int.Parse(e.data["playerId"].ToString()));
    }

    string JsonToString(string target, string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[1];
    }
}
