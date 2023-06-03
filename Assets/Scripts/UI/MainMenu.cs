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
        // YarnCommands.SaveString("Dwayne", "false");
        // pop = Resources.Load<AudioClip>("SFX/Pop");
        if (SaveSystem.Data == null)
        {
            GameObject load = GameObject.Find("LoadButton");
            load.GetComponent<Button>().interactable = false;
            // load.GetComponent<Image>().color = Color.gray;
        }
    }

    public void NewGame()
    {
        // MusicPlayer.audioSource.PlayOneShot(pop);
        // if (PlayerPrefs.GetString("MainSave", "") == "")
        // {
        //     blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(Click.LoadYarnScene("NewGame")));
        // }
        // else
        // {
        //     blackScreen.DOFade(1f, 0.5f).OnComplete( () => StartCoroutine(Click.LoadYarnScene("Intro")));
        // }
        
        blackScreen.DOFade(1f, 0.5f).OnComplete( () => SceneManager.LoadScene("Intro"));
        PlayerPrefs.DeleteAll();
    }

    public void LoadGame()
    {
        // MusicPlayer.audioSource.PlayOneShot(pop);
        // Inventory.LoadGame();
        // blackScreen.DOFade(1f, 0.5f).OnComplete( () => { 
        //     SceneManager.LoadScene("MapScene");
        //     YarnCommands.PlayMusic("Music/Gameplay");
        // });

        blackScreen.DOFade(1f, 0.5f).OnComplete( () => SceneManager.LoadScene(SaveSystem.Data.playerScene));
    }

    public void QuitGame()
    {
        // MusicPlayer.audioSource.PlayOneShot(pop);
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
            Application.OpenURL("https://olaycolay.itch.io/");
        #else
            Application.Quit();
        #endif
    }
}
