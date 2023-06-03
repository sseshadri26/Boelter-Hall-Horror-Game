using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Notification : MonoBehaviour
{
    public static Notification instance {get; private set;}

    private TextMeshProUGUI tm;
    private Sequence sequence;
    
    void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this) 
        { 
            Destroy(this);
            return;
        } 
        instance = this;

        tm = GetComponent<TextMeshProUGUI>();

        tm.alpha = 0f;
    }

    public void ShowMessage(string message, float displayTime = 3f, float fadeTime = 0.5f)
    {
        tm.text = message;
        // tm.alpha = 0f;

        sequence.Complete();
        sequence = DOTween.Sequence().Append(tm.DOFade(1f, fadeTime)).AppendInterval(displayTime - fadeTime*2f).Append(tm.DOFade(0f, fadeTime));
    }
}
