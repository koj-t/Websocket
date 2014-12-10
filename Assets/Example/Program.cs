using UnityEngine;
using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace Example3
{
	public class Program : MonoBehaviour
	{
		private HttpServer httpsv;

		[SerializeField]
		private bool chat=false;
		public void Start ()
		{
			/* Create a new instance of the HttpServer class.
       			*
       			* If you would like to provide the secure connection, you should create the instance
       			* with the 'secure' parameter set to true.
       		*/
			//httpsv = new HttpServer (4649);
			httpsv = new HttpServer (4649, true);

			// To provide the secure connection.
      			//var cert = ConfigurationManager.AppSettings["ServerCertFile"];
      			//var passwd = ConfigurationManager.AppSettings["CertFilePassword"];
      			httpsv.SslConfiguration.ServerCertificate = new X509Certificate2 ("root/import.pfx", "unity");
      		//

			// To set the document root path.
//			httpsv.RootPath = ConfigurationManager.AppSettings ["RootPath"];
			httpsv.RootPath = "root";
			// To set the HTTP GET method event.
			httpsv.OnGet += (sender, e) => {
				var req = e.Request;
				var res = e.Response;

				var path = req.RawUrl;

				if (path == "/"){
					path += "index.html";
					if(chat)path="/chat.html";
				}
				Debug.Log("req.RawUrl:"+path);
				var content = httpsv.GetFile (path);
				if (content == null) {
					res.StatusCode = (int)HttpStatusCode.NotFound;
					return;
				}

				if (path.EndsWith (".html")) {
					res.ContentType = "text/html";
					res.ContentEncoding = Encoding.UTF8;
					//Debug.Log("EndWith:.html");
				}

				res.WriteContent (content);
				//Debug.Log(content);
			};

			// Add the WebSocket services.
			httpsv.AddWebSocketService<Echo> ("/Echo");
			httpsv.AddWebSocketService<Chat> ("/Chat");

			httpsv.Start ();
			if (httpsv.IsListening) {
				Debug.Log ("Listening on port "+httpsv.Port+", and providing WebSocket services:");
				foreach (var path in httpsv.WebSocketServices.Paths)
					Debug.Log ("WebSocketServices.Paths:"+path);
			}

		}
		void OnGUI (){
			if (GUI.Button (new Rect (10, 10, 100, 20), "stop")) {
				httpsv.Stop ();
			}
		}

		void OnApplicationQuit (){
			httpsv.Stop ();
		}
	}
}
