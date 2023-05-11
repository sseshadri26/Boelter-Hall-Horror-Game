using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMap : MonoBehaviour, IAction
{
    // Enables map related stuff when interacting with a map

    public GameObject minimap;
    public Camera camera;
    public RenderTexture texture;

    public void Activate()
    {
        camera.targetTexture = texture;
        minimap.SetActive(true);
    }
}
