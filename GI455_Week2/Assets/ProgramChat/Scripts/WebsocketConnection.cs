using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace ProgramChat
{
    public class WebsocketConnection : MonoBehaviour
    {
        private WebSocket websocket;
        public InputField inputField;
        public Text textDisplay;

        string msgClient;
     

        // Start is called before the first frame update
        void Start()
        {
            //127.0.0.1
            //192.168.1.9
            websocket = new WebSocket("ws://127.0.0.1:65000/");

            websocket.OnMessage += OnMessage;

            websocket.Connect();

            //websocket.Send("I'm coming here.");
        }

        // Update is called once per frame
        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Return))
            //{
            //    websocket.Send("Random number :" + Random.Range(0, 9999));
            //}
        }
        
        private void OnDestroy()
        {
            if(websocket != null)
            {
                websocket.Close();
            }
        }

        public void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {   
            textDisplay.text += messageEventArgs.Data + "\n";
            Debug.Log("Recieve msg : " + messageEventArgs.Data);
        }

        public void textSend()
        {
            msgClient = inputField.text;
            websocket.Send(msgClient);
        }
    }
}

