using System.Linq;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Whiteboard : MonoBehaviour
{
    private int textureSize = 1024;
    private int penSize = 4;
    private int penSizeD2 = 3;
    private Texture2D texture;
    private Color[] color;
    new Renderer renderer;

    private bool touching, touchingLastFrame;
    private float posX, posY;
    private float lastX, lastY;
    bool everyOthrFrame;

    //The bigger whiteboards to copy what is drawn in the small one.
    public Renderer[] otherWhiteboars;
    [HideInInspector] public PhotonView pv;

    private Texture2D receivedTexture;
    [Tooltip("The time it takes to send the whiteboard info through the network, to students.")]
    public float refreshRate = 0.5f;
    float counter = 0;
    bool count;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

        texture.filterMode = FilterMode.Trilinear;
        texture.anisoLevel = 3;

        renderer.material.mainTexture = texture;

        foreach (var item in otherWhiteboars)
        {
            item.material.mainTexture = texture;
        }

        pv = GetComponent<PhotonView>();
    }

    //The code below handles drawing in the whiteboard
    /*
    void Update()
    {
        if (!pv.IsMine) return;
        int x = (int)(posX * textureSize - penSizeD2);
        int y = (int)(posY * textureSize - penSizeD2);

        if (touchingLastFrame)
        {
            texture.SetPixels(x, y, penSize, penSize, color);

            for (float t = 0.01f; t < 1.00f; t += 0.1f)
            {
                int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }
            if (!everyOthrFrame)
            {
                everyOthrFrame = true;
            }
            else if (everyOthrFrame)
            {
                texture.Apply();
                everyOthrFrame = false;
            }
        }

        lastX = (float)x;
        lastY = (float)y;

        //This code below handles that the whiteboard texture is sync'd through the network x seconds(refreshRate) after the last marker touch
        if (counter >= refreshRate)
        {
            StartCoroutine(SendTextureOverNetwork());
            counter = 0;
            count = false;
        }
        if (touchingLastFrame && !touching)
            count = true;

        if (!touchingLastFrame && touching)
        {
            count = false;
            counter = 0;
        }

        if (count && !touching)
            counter += Time.deltaTime;

        touchingLastFrame = touching;
    }
    */

    //RPC sent by the Marker class so every user gets the information to draw in whiteboard.
    [PunRPC]
    public void DrawAtPosition(float[] pos, int _pensize, float[] _color)
    {
        SetPenSize(_pensize);
        color = SetColor(new Color(_color[0], _color[1], _color[2]));

        int x = (int)(pos[0] * textureSize - penSizeD2);
        int y = (int)(pos[1] * textureSize - penSizeD2);

        //If last frame was not touching a marker, we don't need to lerp from last pixel coordinate to new, so we set the last coordinates to the new.
        if (!touchingLastFrame)
        {
            lastX = (float)x;
            lastY = (float)y;
            touchingLastFrame = true;
        }

        if (touchingLastFrame)
        {
            texture.SetPixels(x, y, penSize, penSize, color);

            //Lerp last pixel to new pixel, so we draw a continuous line.
            for (float t = 0.01f; t < 1.00f; t += 0.1f)
            {
                int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }
            //We apply the texture every other frame, so we improve performance.
            if (!everyOthrFrame)
            {
                everyOthrFrame = true;
            }
            else if (everyOthrFrame)
            {
                texture.Apply();
                everyOthrFrame = false;
            }
        }

        lastX = (float)x;
        lastY = (float)y;
    }

    //Reset the state of the whiteboard, so it doesn't interpolate/lerp last pixels drawn.
    [PunRPC]
    public void ResetTouch()
    {
        touchingLastFrame = false;
    }

    //Called from Marker.cs
    public void ToggleTouch(bool touching)
    {
        this.touching = touching;
    }

    //Receives the position of the raycast from the marker to the whiteboard
    public void SetTouchPosition(float x, float y)
    {
        posX = x;
        posY = y;
    }

    //Receives the color from the marker
    public Color[] SetColor(Color color)
    {
        return Enumerable.Repeat(color, penSize * penSize).ToArray();
    }

    public void ClearWhiteboard()
    {
        Debug.Log("Clearing whiteboard");
        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        renderer.material.mainTexture = texture;

        foreach (var item in otherWhiteboars)
        {
            item.material.mainTexture = texture;
        }
        counter = 0;
        count = false;
        texture.Apply();
    }

    public void SetPenSize(int n)
    {
        penSize = n;
        penSizeD2 = n / 2;
    }

    //Sends the texture through the network using a photon RPC, to everyone else (RPCTarget.Others)
    IEnumerator SendTextureOverNetwork()
    {
        pv.RPC("Send", RpcTarget.OthersBuffered, texture.EncodeToPNG());
        yield return null;
    }

    [PunRPC]
    private void Send(byte[] receivedByte)
    {
        receivedTexture = new Texture2D(1, 1);
        receivedTexture.LoadImage(receivedByte);
        ApplyReceivedTexture();
    }


    void ApplyReceivedTexture()
    {
        foreach (var item in otherWhiteboars)
        {
            item.material.mainTexture = receivedTexture;
        }
        GetComponent<Renderer>().material.mainTexture = receivedTexture;
    }
}
