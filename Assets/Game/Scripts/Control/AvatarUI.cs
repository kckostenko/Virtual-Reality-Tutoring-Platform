using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class AvatarUI : MonoBehaviour
    {
        [SerializeField] GameObject[] Heads;
        [SerializeField] GameObject[] Bodies;
        [SerializeField] Transform headLocation;
        [SerializeField] Transform bodyLocation;

        int headIndex = 0;
        int bodyIndex = 0;

        public void OnClick_ChangeHeadForward()
        {
            headIndex++;
            if (headIndex >= Heads.Length)
                headIndex = 0;
            Destroy(headLocation.GetChild(0).gameObject);
            Instantiate(Heads[headIndex], headLocation);
            PlayerManager.Instance.SetHead(Heads[headIndex]);
        }

        public void OnClick_ChangeHeadBackward()
        {
            headIndex--;
            if (headIndex == -1)
                headIndex = Heads.Length - 1;
            Destroy(headLocation.GetChild(0).gameObject);
            Instantiate(Heads[headIndex], headLocation);
            PlayerManager.Instance.SetHead(Heads[headIndex]);
        }

        public void OnClick_ChangeBodyForward()
        {
            bodyIndex++;
            if (bodyIndex >= Bodies.Length)
                bodyIndex = 0;
            Destroy(bodyLocation.GetChild(0).gameObject);
            Instantiate(Bodies[bodyIndex], bodyLocation);
            PlayerManager.Instance.SetBody(Bodies[bodyIndex]);
        }

        public void OnClick_ChangeBodyBackward()
        {
            bodyIndex--;
            if (bodyIndex == -1)
                bodyIndex = Bodies.Length - 1;
            Destroy(bodyLocation.GetChild(0).gameObject);
            Instantiate(Bodies[bodyIndex], bodyLocation);
            PlayerManager.Instance.SetBody(Bodies[bodyIndex]);
        }
    }
}