using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Game.Control
{
    public class BodyController : MonoBehaviour
    {
        PhotonView pv;
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

            GameObject player = GameObject.Find(playerName);
            if (player == null) return;
            AvatarController avatar = player.GetComponentInChildren<AvatarController>();
            Transform body = avatar.transform.Find("Body");
            transform.SetParent(body, false);

        }
    }
}