using UnityEngine;
using System.Collections;
using WebSocketSharp;


public class VoiceRecog: MonoBehaviour {
	private WebSocket ws;

	void Awake () {
		ws = new WebSocket ("ws://127.0.0.1:12001");
		ws.OnMessage += (sender,e) => {
			Debug.Log (e.Data);
		};
		ws.Connect ();
	}

	void OnApplicationQuit(){
		ws.Close ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
