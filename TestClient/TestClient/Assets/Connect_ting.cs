using UnityEngine; 
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Connect_ting : MonoBehaviour {

	private SocketIOComponent socket;

	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		StartCoroutine("ConnectToServer");

        socket.On("USER_CONNECTED", OnUserConnected);
		socket.On("PLAY", OnUserPlay);
		socket.On("USER_DISCONNECTED", OnUserDisconnected);
		socket.On("ROLL_DICE", OnRollDice);
		socket.On("SEND_CARDS", OnReceiveCards);
        socket.On("CHECK_CARD", OnReceiveCheck);
		socket.On("ROOMS", OnReceiveRooms);


    }

	IEnumerator ConnectToServer(){
		
		yield return new WaitForSeconds(0.5f);
		socket.Emit("USER_CONNECT");
		Dictionary<string,string> data = new Dictionary<string, string>();
		data ["name"] = "UNITY";
		JSONObject jso = new JSONObject(data);

		yield return new WaitForSeconds(1f);
		socket.Emit("PLAY", jso);
	}

    //draw card 
	public void DrawCards()
    {

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["number"] = "1";
        data["type"] = "word";
        JSONObject cardjso = new JSONObject(data);
        socket.Emit("DRAW_CARDS", cardjso);

    }

    public void PlayCards() {

        Dictionary<string, string> data = new Dictionary<string, string>();
        //Array?
        data["id"] = "test01";
        JSONObject cardjso = new JSONObject(data);
        socket.Emit("PLAY_CARD", cardjso);

    }


	public void ListRoom() {
		socket.Emit ("LIST_ROOMS");
	}

	public void OnUserConnected(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] OnUserConnect received: "  + e.data);
	}

	public void OnUserDisconnected(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] OnUserDisconnected received: "  + e.data);
	}
        
	public void OnUserPlay(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] OnUserPlay: " + e.data);
	}

    public void OnRollDice(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] OnRollDice:" + e.data);
    }

	public void OnReceiveCards(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] OnReceiveCards:" + e.data);
    }

    public void OnReceiveCheck(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] OnReceiveCheck:" + e.data);
        /*
		if (e.data == "true")
        {
            Debug.log("Done!");
        }
        else if (e.data == "false")
        {
            Debug.log("NO!!");
        }
        else {
            Debug.log("Exception");
        }
        */

    }

	public void  OnReceiveRooms(SocketIOEvent e){

		Debug.Log("[SocketIO] OnReceiveListRoom:" + e.data);
	}

    // Update is called once per frame
    void Update () {
	
	}
}