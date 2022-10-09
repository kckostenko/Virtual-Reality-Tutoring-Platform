using HurricaneVR.Framework.Core;
using Photon.Pun;
using Smooth;
using UnityEngine;

namespace Game.Network
{
    public class TransferOnTouchPUN : MonoBehaviour
    {
        [SerializeField] PhotonView[] pv;
        HVRGrabbable grabbable;
        int owner = -1;

        private void Start()
        {
            if (pv.Length == 0)
            {
                pv = new PhotonView[1];
                pv[0] = GetComponent<PhotonView>();
            }
            grabbable = GetComponent<HVRGrabbable>();
        }

        private void OnCollisionStay(Collision collision)
        {
            // If I don't have a grabbable or I'm being held by someone already, then stop
            if (grabbable == null || grabbable.IsBeingHeld ) return;

            // If what touched me is a Hand then it should own me
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hand"))
            {
                if (owner == collision.gameObject.GetComponent<PhotonView>().OwnerActorNr)
                    return;
                TransferOwnership(collision.gameObject.GetComponent<PhotonView>().OwnerActorNr);
            }

            TransferOnTouchPUN collisionTOT = collision.gameObject.GetComponent<TransferOnTouchPUN>();
            if (collisionTOT == null || owner == collisionTOT.owner) return;

            // If the object that touched me is a grabbable and is being held right now, then its owner should be my owner too
            // (Example: I am a ball being touched by a racket ball held by a player, the player should own me too)
            if (collisionTOT.grabbable && collisionTOT.grabbable.IsBeingHeld)
            {
                TransferOwnership(collisionTOT.owner);
            }
        }

        void TransferOwnership(int newOwner)
        {
            owner = newOwner;
            GetComponent<SmoothSyncPUN2>().clearBuffer();
            foreach (PhotonView view in pv)
                view.TransferOwnership(newOwner);

            //pv[0].RPC("SmoothSyncClearBuffer", RpcTarget.AllBuffered, newOwner);
        }

        [PunRPC]
        void SmoothSyncClearBuffer(int newOwner)
        {
            owner = newOwner;
            GetComponent<SmoothSyncPUN2>().clearBuffer();
            foreach (PhotonView view in pv)
                view.TransferOwnership(newOwner);
        }
    }
}