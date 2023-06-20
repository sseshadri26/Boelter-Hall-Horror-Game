using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image blackScreen;
    private AudioClip pop;

    private void Awake()
    {
        if (!SaveSystem.HasSavedgame())
        {
            GameObject load = GameObject.Find("LoadButton");
            load.GetComponent<Button>().interactable = false;
            // load.GetComponent<Image>().color = Color.gray;
        }

        // Reset all static variables so that starting a new game starts with a blank slate
        Globals.Reset();
    }

    public void NewGame()
    {
        blackScreen.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene("Intro"));
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (SaveSystem.HasSavedgame())
        {
            SaveSystem.LoadGame();
            blackScreen.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(SaveSystem.GetLoadedScene()));
        }
    }

    public void QuitGame()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_WEBGL)
            Application.OpenURL("https://olaycolay.itch.io/");
#else
            Application.Quit();
#endif
    }
}
