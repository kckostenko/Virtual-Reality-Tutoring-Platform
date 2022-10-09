using UnityEngine;
using Photon.Pun;
using HurricaneVR.Framework.Core;
using System.Collections;

//This class sends a Raycast from the marker and detect if it's hitting the whiteboard (tag: Finish)
public class Marker : MonoBehaviour
{
    [SerializeField] int penSize = 8;
    [SerializeField] Color color = Color.blue;

    private Whiteboard whiteboard;
    public Transform drawingPoint;
    public Renderer markerTip;
    private RaycastHit touch;
    bool touching;
    bool busy = false;
    float drawingDistance = 0.04f;
    
    PhotonView pv;
    HVRGrabbable grabbable;

    
    bool grabbed;

    Quaternion lastAngle;
    float lastY;

    public void ToggleGrab(bool b)
    {
        if (b) grabbed = true;
        else grabbed = false;
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        if(pv == null)
            pv = transform.parent.GetComponent<PhotonView>();
        grabbable = GetComponent<HVRGrabbable>();
        if (grabbable == null)
            grabbable = transform.parent.GetComponent<HVRGrabbable>();

        markerTip.material.color = color;
    }

    void Update()
    {
        if (!pv.IsMine) return;
        if (!grabbable.IsBeingHeld) return;


        if (Physics.Raycast(drawingPoint.position, drawingPoint.up, out touch, drawingDistance))
        {
            if (touch.collider.CompareTag("Board"))
            {
                if (!touching)
                {
                    touching = true;
                    lastY = transform.position.y;
                    lastAngle = transform.rotation;
                    whiteboard = touch.collider.GetComponent<Whiteboard>();
                }
                if (whiteboard == null) return;
                StartCoroutine(Draw());
            }
        }
        else if (whiteboard != null)
        {
            touching = false;
            whiteboard.pv.RPC("ResetTouch", RpcTarget.AllBuffered);
            whiteboard = null;
        }
    }

    IEnumerator Draw()
    {
        if (busy) yield break;

        busy = true;
        whiteboard.pv.RPC("DrawAtPosition", RpcTarget.AllBuffered, new float[] { touch.textureCoord.x, touch.textureCoord.y }, penSize, new float[] { color.r, color.g, color.b });
        yield return new WaitForSeconds(penSize/500f * Time.deltaTime);
        busy = false;
    }

    /*
    private void LateUpdate()
    {
        if (!pv.IsMine) return;

        //lock rotation of marker when touching whiteboard
        if (touching)
        {
            //if(transform.position.y > lastY)
            //    transform.position = new Vector3(transform.position.x, lastY, transform.position.z);
            transform.rotation = lastAngle;
        }
    }
    */

}
