using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Network
{
    public class UserHandUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] PhotonView player;
        [SerializeField] PhotonVoiceView voice;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] GameObject crown;
        [SerializeField] GameObject muteButton;
        [SerializeField] GameObject unmuteButton;
        [SerializeField] GameObject lockButton;

        void Start()
        {
            nameText.text = player.Owner.NickName;

            if (player.Owner.NickName == PhotonNetwork.MasterClient.NickName)
            {
                crown.SetActive(true);
                lockButton.SetActive(true);
            }
            else
                muteButton.SetActive(true);
        }

        public void OnClick_Mute()
        {
            voice.RecorderInUse.TransmitEnabled = false;
        }

        public void OnClick_Unmute()
        {
            voice.RecorderInUse.TransmitEnabled = true;
        }

        public void OnClick_Lock()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        public void OnClick_Unlock()
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

        public void OnClick_Leave()
        {
            StartCoroutine(Leave());
        }

        IEnumerator Leave()
        {
            PhotonNetwork.LeaveRoom();
            //DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(0);
            //PhotonNetwork.Disconnect();
            //Destroy(gameObject);
        }



        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (player.Owner.NickName == PhotonNetwork.MasterClient.NickName)
            {
                if(player.Owner.NickName == PhotonNetwork.NickName)
                    OnClick_Unmute();
                muteButton.SetActive(false);
                unmuteButton.SetActive(false);
                crown.SetActive(true);
                lockButton.SetActive(true);
            }
        }

    }

}