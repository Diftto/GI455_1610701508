using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocket
{
    public class WebSocketConnection : MonoBehaviour
    {
        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        private WebSocket ws;

        private string tempMessageString;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;

        public GameObject pageLogin;
        public GameObject pageCreateOrJoin;
        public GameObject pageCreateRoom;
        public GameObject pageJoinRoom;
        public GameObject pageinRoom;

        public GameObject pageTryagainJoin;
        public GameObject pageTryagainCreate;

        public InputField yourName;
        string Name;
        public InputField nameroomCreate;
        public InputField nameroomJoin;
        string NameJoin;

        public Text shownameroom;

        private void Update()
        {
            UpdateNotifyMessage();
        }

        public void Connect()
        {
            string url = "ws://127.0.0.1:65000/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            Name = yourName.text;
            print(Name);

            pageLogin.SetActive(false);
            pageCreateOrJoin.SetActive(true);

        }

        public void CreateRoom(string roomName)
        {
            shownameroom.text = "NameRoom is " + nameroomCreate.text;
            roomName = nameroomCreate.text;

            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
         
        }

        public void JoinRoom(string roomName)
        {
            shownameroom.text = "NameRoom is " + nameroomJoin.text;
            roomName = nameroomJoin.text;
            
            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

        }

        public void ErrorJoin()
        {
            pageTryagainJoin.SetActive(false);
        }

        public void ErrorCreate()
        {
            pageTryagainCreate.SetActive(false);
        }

        public void LobbyCreate()
        {
            pageCreateOrJoin.SetActive(false);
            pageCreateRoom.SetActive(true);
        }

        public void LobbyJoin()
        {
            pageCreateOrJoin.SetActive(false);
            pageJoinRoom.SetActive(true);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            pageinRoom.SetActive(false);
            pageLogin.SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        public void SendMessage(string message)
        {

        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);

                    if(receiveMessageData.data == "fail")
                    {
                        pageTryagainCreate.SetActive(true);
                    }
                    else
                    {
                        pageCreateRoom.SetActive(false);
                        pageinRoom.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);

                    if(receiveMessageData.data == "true")
                    {
                        pageTryagainJoin.SetActive(true);
                     
                    }
                    else
                    {
                        pageJoinRoom.SetActive(false);
                        pageinRoom.SetActive(true);
                    }
                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }
    }
}



