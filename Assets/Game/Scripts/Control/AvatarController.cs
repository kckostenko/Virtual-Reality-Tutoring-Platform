using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Voice.PUN;

namespace Game.Control
{
    public class AvatarController : MonoBehaviourPunCallbacks
    {
        [SerializeField] Transform ovrHead;
        [SerializeField] Transform avatarHead;
        [SerializeField] Transform avatarBody;

        [SerializeField] Vector3 headPositionOffset;
        [Header("Recorder")]
        [SerializeField] PhotonVoiceView photonVoice;
        [SerializeField] SpriteRenderer recorderSprite;

        PhotonView pv;
        bool isSpeaking = false;


        private void Start()
        {
            pv = GetComponent<PhotonView>();

            if (transform.parent.GetComponent<PhotonView>().IsMine)
            {
                print("AvatarController::Start --> IsMine got activated");
                PhotonNetwork.Instantiate(PlayerManager.Instance.GetHead().name, Vector3.zero, Quaternion.identity);
                PhotonNetwork.Instantiate(PlayerManager.Instance.GetBody().name, Vector3.zero, Quaternion.identity);
            }
        }

        private void Update()
        {
            Vector3 relativeOffset = headPositionOffset.y * ovrHead.up + headPositionOffset.z * ovrHead.forward;
            avatarHead.position = Vector3.Lerp(avatarHead.position, ovrHead.position + relativeOffset, 0.5f);
            avatarBody.position = avatarHead.position - Vector3.up * 0.4f;
            avatarHead.rotation = Quaternion.Lerp(avatarHead.rotation, ovrHead.rotation, 0.5f);
            avatarBody.rotation = Quaternion.Lerp(avatarBody.rotation, Quaternion.Euler(new Vector3(0, avatarHead.rotation.eulerAngles.y, 0)), 0.05f);

            
            if(pv.IsMine)
            {
                isSpeaking = photonVoice.IsRecording;
                if (isSpeaking != recorderSprite.enabled)
                    pv.RPC("ToggleSpeaking", RpcTarget.All, isSpeaking);
            }

        }

        [PunRPC]
        void ToggleSpeaking(bool toggle)
        {
            recorderSprite.enabled = toggle;
        }

    }
}
