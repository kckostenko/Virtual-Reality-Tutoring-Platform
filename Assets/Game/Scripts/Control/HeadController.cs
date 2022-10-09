using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Game.Control
{
    public class HeadController : MonoBehaviour
    {
        PhotonView pv;
        GameObject player = null;
        
        string playerName = "";

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            playerName = pv.Owner.NickName;
        }


        private void Update()
        {

            if (playerName == "") return;
            if (transform.parent != null) return;

            if(player == null) player = GameObject.Find(playerName);
            if (player == null) return;

            AvatarController avatar = player.GetComponentInChildren<AvatarController>();
            Transform head = avatar.transform.Find("Head");
            transform.SetParent(head, false);
        }
    }
}