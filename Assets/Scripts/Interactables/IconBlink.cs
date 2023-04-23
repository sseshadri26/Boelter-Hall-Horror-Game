using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBlink : MonoBehaviour
{
    //Toggles an object on and off every second; time can be adjusted in
    //InvokeRepeating()
    public GameObject objectToToggle;
    private bool isToggled = false;

    void Start()
    {
        InvokeRepeating("ToggleObject", 0f, 1.0f);
    }

    void ToggleObject()
    {
        objectToToggle.SetActive(!isToggled);
        isToggled = !isToggled;
    }
}
