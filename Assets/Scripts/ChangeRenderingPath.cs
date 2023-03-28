using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderingPath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeRenderingPathDelayed", 1f);
        Invoke("ChangeRenderingPathDelayed2", 2f);
        
    }

    void ChangeRenderingPathDelayed()
    {
        Camera.main.renderingPath = RenderingPath.Forward;
    }
    void ChangeRenderingPathDelayed2()
    {
        Camera.main.renderingPath = RenderingPath.UsePlayerSettings;
    }
}
