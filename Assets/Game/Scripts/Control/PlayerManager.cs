using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] GameObject head;
        [SerializeField] GameObject body;

        public static PlayerManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = GetComponent<PlayerManager>();
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void SetHead(GameObject newHead)
        {
            head = newHead;
        }

        public void SetBody(GameObject newBody)
        {
            body = newBody;
        }

        public GameObject GetHead()
        {
            return head;
        }

        public GameObject GetBody()
        {
            return body;
        }


    }

}