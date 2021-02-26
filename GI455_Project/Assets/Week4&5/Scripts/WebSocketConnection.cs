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
        //----week5----//
        public InputField userID;
        public InputField password;

        public InputField regisuserID;
        public InputField regisName;
        public InputField regisPassword;
        public InputField regisRePassword;

        public InputField textMessage;

        public GameObject pageLoginOrRegis;
        public GameObject pageRegister;
        public GameObject pageErrorInput;
        public GameObject pageErrorLogin;
        public GameObject pageErrorPassword;
        public GameObject pageErrorRegis;

        public Text ShowNameUser;
        string NameUser;

        public Text DisplayClientMessage;
        public Text DisplayOthersMessage;
        string otherMessage;

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
            pageLoginOrRegis.SetActive(true);
            print("Client is Connect.");

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

        public void Login(string userLogin)
        {

            userLogin = userID.text + "#" + password.text;

            if (userID.text == "" || password.text == "")
            {
                pageErrorInput.SetActive(true);
               
                print("input All plz");
            }
            else if(userID.text != "")
            {
                if (password.text != "")
                {
                    SocketEvent socketEvent = new SocketEvent("Login", userLogin);

                    string toJsonStr = JsonUtility.ToJson(socketEvent);

                    ws.Send(toJsonStr);
                }
            }

        }

        public void IntoRegis()
        {
            pageLoginOrRegis.SetActive(false);
            pageRegister.SetActive(true);
        }

        public void OKErrInput()
        {
            pageErrorInput.SetActive(false);
        }

        public void OKErrLogin()
        {
            pageErrorLogin.SetActive(false);
        }

        public void OKErrPassword()
        {
            pageErrorPassword.SetActive(false);
        }
        public void OKErrRegister()
        {
            pageErrorRegis.SetActive(false);
        }

        public void Register(string userRegis)
        {
            userRegis = regisuserID.text + "#" + regisName.text + "#" + regisPassword.text;

            if (regisuserID.text == "" || regisName.text == "" || regisPassword.text == "" || regisRePassword.text == "")
            {
                pageErrorInput.SetActive(true);
                
                print("Some Input is Null.");
            }
            else if(regisPassword.text != regisRePassword.text)
            {
                pageErrorPassword.SetActive(true);
              
                print("Password and Repassword don't macth.");
            }
            else if (regisuserID.text != "")
            {
                if (regisName.text != "")
                {
                    if (regisPassword.text != "")
                    {
                        if (regisRePassword.text != "")
                        {
                            print("Client Register success.");
                            SocketEvent socketEvent = new SocketEvent("Register", userRegis);

                            string toJsonStr = JsonUtility.ToJson(socketEvent);

                            ws.Send(toJsonStr);
                        }
                    }
                }
            }
        }

        public void MessageSend(string clientMessage)
        {
            clientMessage = textMessage.text;

            SocketEvent socketEvent = new SocketEvent("SendMessage", clientMessage + "#" + NameUser);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            DisplayOthersMessage.text += "" + "\n" + "";
            DisplayClientMessage.text += "\n" + NameUser + " : " + clientMessage;

            ws.Send(toJsonStr);
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
                else if(receiveMessageData.eventName == "Login")
                {
                    if(receiveMessageData.data == "fail")
                    {
                        pageErrorLogin.SetActive(true);
                    }
                    else
                    {
                        NameUser = receiveMessageData.data;
                        ShowNameUser.text = NameUser;

                        pageCreateOrJoin.SetActive(true);
                        pageLoginOrRegis.SetActive(false);
                    }
                }
                else if (receiveMessageData.eventName == "Register")
                {
                    if (receiveMessageData.data == "fail")
                    {
                        pageErrorRegis.SetActive(true);
                    }
                    else if(receiveMessageData.data == "success")
                    {
                        pageLoginOrRegis.SetActive(true);
                        pageRegister.SetActive(false);
                    }
                }
                else if(receiveMessageData.eventName == "SendMessage")
                {
                    var splitStr = receiveMessageData.data.Split('#');
                    var message = splitStr[0];
                    var username = splitStr[1];
                    /*if (NameUser == ShowNameUser.text)
                    {
                        DisplayClientMessage.text = NameUser + " : " + message;
                    }
                    else */
                    if(NameUser != username)
                    {
                        DisplayClientMessage.text += "" + "\n" + "";
                        DisplayOthersMessage.text += "\n" + username + " : " + message;
                    }
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



