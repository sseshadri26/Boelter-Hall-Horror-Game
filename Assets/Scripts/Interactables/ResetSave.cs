using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSave : MonoBehaviour, IAction
{    
    // Reset Save and an Interactable for devs only
    private void Awake()
    {
        // This object should only exist for debugging
        if (!Application.isEditor && !Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        PlayerPrefs.DeleteAll();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
        Application.OpenURL("olaycolay.itch.io");
        #else
        Application.Quit();
        #endif
    }
}
