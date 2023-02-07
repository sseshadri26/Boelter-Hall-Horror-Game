using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For loading next scene on interact.
public class ToNextScene : MonoBehaviour, IAction
{
    private bool activated;
    void Start()
    {
        
    }

    // If door is activated, load next scene.
    public void Activate()
    {
        activated = !activated;
        if(activated)
        {
            SceneManager.LoadScene("Demo");
        }
        else {}
    }
}
