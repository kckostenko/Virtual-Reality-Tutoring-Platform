using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Network
{
    public class SetPlayerName : MonoBehaviour
    {
        [SerializeField] PhotonView playerView;
        void Start()
        {
            GetComponent<TextMeshProUGUI>().text = playerView.Owner.NickName;
        }

    }
}
