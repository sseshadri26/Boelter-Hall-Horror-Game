using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChanger : MonoBehaviour, IAction
{
    [SerializeField]
    public Material newTextureMaterial;

    [SerializeField]
    public GameObject ObjectToChange;

    private Renderer objectRenderer;

    public void Activate()
    {
        ObjectToChange.SetActive(true);
    }
}
