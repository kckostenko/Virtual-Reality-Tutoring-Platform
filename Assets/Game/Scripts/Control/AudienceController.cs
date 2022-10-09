using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Control
{
    public class AudienceController : MonoBehaviour
    {
        [SerializeField] MeshRenderer head;
        [SerializeField] MeshRenderer body;
        [SerializeField] MeshRenderer face;

        Material bodyMaterial;
        Material faceMaterial;
        MeshRenderer meshRenderer;
        Animator animator;
        bool busy = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            bodyMaterial = new Material(Shader.Find("Standard"));
            faceMaterial = new Material(Shader.Find("Standard"));
            bodyMaterial.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            faceMaterial.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            head.material = bodyMaterial;
            body.material = bodyMaterial;
            face.material = faceMaterial;
        }

        void Update()
        {
            if (busy) return;

            int behaviour = Random.Range(0, 9);

            if(behaviour >=0 && behaviour <= 4)
                StartCoroutine(Wait());
            else if (behaviour == 5)
                StartCoroutine(StartBehaviour("nod"));
            else if (behaviour == 6)
                StartCoroutine(StartBehaviour("lookleft"));
            else if (behaviour == 7)
                StartCoroutine(StartBehaviour("lookright"));
            else if (behaviour == 8)
                StartCoroutine(StartBehaviour("nodtwice"));
            else if (behaviour == 9)
                StartCoroutine(StartBehaviour("shakehead"));
        }

        IEnumerator StartBehaviour(string behaviour)
        {
            busy = true;
            animator.SetTrigger(behaviour);
            yield return new WaitForSeconds(1);
            busy = false;
        }

        IEnumerator Wait()
        {
            busy = true;
            yield return new WaitForSeconds(1);
            busy = false;
        }
    }

}