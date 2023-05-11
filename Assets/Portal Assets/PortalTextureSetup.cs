using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{


    public Camera cameraA;
    public Camera cameraB;


    public Material cameraMatA;
    public Material cameraMatB;

    public MeshRenderer screenA;
    public MeshRenderer screenB;

    public Shader ScreenCutoutShader;

    // Use this for initialization
    void Awake()
    {

        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }
        float ppppp = 1;
        //cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        //cameraA.targetTexture = new RenderTexture(384, 216, 24);
        cameraA.targetTexture = new RenderTexture((int)(384 / ppppp), (int)(216 / ppppp), 24);


        cameraMatA = new Material(ScreenCutoutShader);
        cameraMatA.mainTexture = cameraA.targetTexture;
        screenB.material = cameraMatA;


        if (cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }
        //cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        //cameraB.targetTexture = new RenderTexture(384, 216, 24);
        cameraB.targetTexture = new RenderTexture((int)(384 / ppppp), (int)(216 / ppppp), 24);


        cameraMatB = new Material(ScreenCutoutShader);
        cameraMatB.mainTexture = cameraB.targetTexture;
        screenA.material = cameraMatB;




    }

}
