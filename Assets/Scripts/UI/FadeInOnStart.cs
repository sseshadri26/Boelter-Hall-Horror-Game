using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOnStart : MonoBehaviour
{
    public float delay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Image>())
        {
            GetComponent<Image>().DOFade(0f, 1f).SetDelay(delay);
        }
        else if (GetComponent<RawImage>())
        {
            GetComponent<RawImage>().DOFade(0f, 1f).SetDelay(delay);
        }
    }
}
