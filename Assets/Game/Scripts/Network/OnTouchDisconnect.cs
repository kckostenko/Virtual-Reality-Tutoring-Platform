using HurricaneVR.Framework.Core.Player;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Network
{
    public class OnTouchDisconnect : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<HVRPlayerController>())
            {
                PhotonNetwork.Disconnect();
            }

        }
    }
}