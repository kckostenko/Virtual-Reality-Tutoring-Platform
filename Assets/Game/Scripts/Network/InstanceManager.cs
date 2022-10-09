using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Integrations.PUN;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Network
{
    public class InstanceManager : MonoBehaviourPunCallbacks
    {
        public GameObject Prefab;
        public GameObject Player;

        void Start()
        {
            if(PhotonNetwork.IsConnectedAndReady)
            {
                Player = PhotonNetwork.Instantiate(Prefab.name, transform.position, transform.rotation);
                HVRManager hvrManager = FindObjectOfType<HVRManager>();
                hvrManager.GrabberManager = GetComponent<HVRPunGrabberManager>();
            }
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
        }
    }

}