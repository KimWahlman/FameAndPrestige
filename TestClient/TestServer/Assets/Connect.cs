using UnityEngine; 
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Connect : MonoBehaviour {

	public SocketIOComponent socket;

	// Use this for initialization
	void Start () {

		StartCoroutine(ConnectToServer());
		socket.On("USER_CONNECTED", OnUserConnected);
		socket.On("PLAY", OnUserPlay);
		socket.On("USER_DISCONNECTED", OnUserDisconnected);
	
	}

	IEnumerator ConnectToServer(){
		
		yield return new WaitForSeconds(0.5f);
		socket.Emit("USER_CONNECT");
		yield return new WaitForSeconds(0.5f);

		Dictionary<string,string> data = new Dictionary<string, string>();
		data ["name"] = "luigi";
		JSONObject jso = new JSONObject(data);
		socket.Emit("PLAY", jso);

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

	// Update is called once per frame
	void Update () {
	
	}
}
