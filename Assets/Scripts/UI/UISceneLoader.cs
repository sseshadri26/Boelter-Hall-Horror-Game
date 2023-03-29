using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A test script to bring UI functionality into a game scene
public class UISceneLoader : MonoBehaviour
{
    void Awake()
    {
        // DESIGN CHOICE: Don't want to do async for the guarantee that UI
        // will be ready before Start is called
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        Screen.SetResolution(384, 216, true, 60);
        Application.targetFrameRate = 60;
    }
}
