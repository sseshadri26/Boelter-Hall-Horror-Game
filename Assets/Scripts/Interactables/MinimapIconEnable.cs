using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconEnable : MonoBehaviour, IAction
{
    /*
    Used for displaying icons on minimap, but could really be used for anything
    */
    [SerializeField]
    public GameObject IconToChange;

    public void Activate()
    {
        IconToChange.SetActive(true);
    }
}