using Game.Objects;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("Panels (Filled Automatically InGame)")]
        public GameObject loginPanel;
        public GameObject connectionPanel;
        public GameObject roomPanel;
        public GameObject environmentsPanel;
        public GameObject joinedPanel;
        [Header("RoomPanel Items (Filled Automatically InGame)")]
        public TMP_InputField createRoomName;
        public TMP_InputField createRoomPassword;
        public TMP_InputField JoinRoomName;
        public TMP_InputField JoinRoomPassword;

        bool roomCreator = false;

        private void Awake()
        {
            NetworkManager[] nms = FindObjectsOfType<NetworkManager>();
            if(nms.Length > 1)
            {
                foreach(NetworkManager m in nms)
                {
                    if (m != this)
                        Destroy(m.gameObject);
                }
            }
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnectedAndReady)
                PhotonNetwork.AutomaticallySyncScene = true;

            GetPanels();
            ResetPanels();
            loginPanel.SetActive(true);
        }

        private void GetPanels()
        {
            GameObject GameUI = GameObject.Find("/Game UI");
            loginPanel = GameUI.transform.GetChild(0).gameObject;
            connectionPanel = GameUI.transform.GetChild(1).gameObject;
            roomPanel = GameUI.transform.GetChild(2).gameObject;
            environmentsPanel = GameUI.transform.GetChild(3).gameObject;
            joinedPanel = GameUI.transform.GetChild(4).gameObject;

            createRoomName = roomPanel.transform.Find("Left Panel/Name InputField").GetComponent<TMP_InputField>();
            createRoomPassword = roomPanel.transform.Find("Left Panel/Password InputField").GetComponent<TMP_InputField>();
            JoinRoomName = roomPanel.transform.Find("Right Panel/Name InputField").GetComponent<TMP_InputField>();
            JoinRoomPassword = roomPanel.transform.Find("Right Panel/Password InputField").GetComponent<TMP_InputField>();

            createRoomName.text = "";
            createRoomPassword.text = "";
            JoinRoomName.text = "";
            JoinRoomPassword.text = "";
        }

        private void ResetPanels()
        {
            loginPanel.SetActive(false);
            connectionPanel.SetActive(false);
            roomPanel.SetActive(false);
            environmentsPanel.SetActive(false);
            joinedPanel.SetActive(false);
        }

        private void Joined()
        {
            ResetPanels();
            if(roomCreator)
                environmentsPanel.SetActive(true);
            else
                joinedPanel.SetActive(true);
            LockedDoor door = FindObjectOfType<LockedDoor>();
            door.Unlock();

        }

        public void OnClick_ConnectToServer(TMP_InputField _nickname)
        {
            string nickname = _nickname.text;

            if (string.IsNullOrEmpty(nickname))
                nickname = "User" + Random.Range(0, 1000).ToString("0000");
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.ConnectUsingSettings();
            ResetPanels();
            connectionPanel.SetActive(true);
        }

        public void OnClick_CreateRoom()
        {
            string name = createRoomName.text;
            string pass = createRoomPassword.text;
            if (string.IsNullOrEmpty(name))
                name = "Room" + Random.Range(0, 9999).ToString("0000");
            if (string.IsNullOrEmpty(pass))
                pass = Random.Range(0, 9999).ToString("0000");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(name, roomOptions);
        }

        public void OnClick_JoinRoom()
        {
            string name = JoinRoomName.text;
            string pass = JoinRoomPassword.text;
            PhotonNetwork.JoinRoom(name);
        }

        public override void OnConnected()
        {
            print("Connected to the internet");
            base.OnConnected();
        }

        public override void OnConnectedToMaster()
        {
            print("Connected to the server using nickname: " + PhotonNetwork.NickName);
            ResetPanels();
            roomPanel.SetActive(true);
            base.OnConnectedToMaster();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            print("Disconnected: " + cause);
            ResetPanels();
            loginPanel.SetActive(true);
            base.OnDisconnected(cause);
        }

        public override void OnCreatedRoom()
        {
            print("User " + PhotonNetwork.NickName + " Created Room " + PhotonNetwork.CurrentRoom);
            roomCreator = true;
            base.OnCreatedRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            print("Failed " + message);
            base.OnCreateRoomFailed(returnCode, message);
        }

        public override void OnJoinedRoom()
        {
            print("User " + PhotonNetwork.NickName + " Joined Room " + PhotonNetwork.CurrentRoom);
            Joined();
            base.OnJoinedRoom();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            print("Failed " + message);
            base.OnJoinRoomFailed(returnCode, message);
        }

    }
}