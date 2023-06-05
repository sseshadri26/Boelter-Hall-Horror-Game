using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(FirstPersonController))]
public class CareyCutscene : MonoBehaviour
{
    public AudioClip careyCallout;

    public AudioClip introMusic;

    private FirstPersonController fpc;

    private void Awake()
    {
        fpc = GetComponent<FirstPersonController>();
    }

    public void NachenbergIntro(TV nachTV)
    {
        fpc.controls.Disable();
        fpc.pitch = 0f;
        StartCoroutine("CareyCallout", nachTV);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(new Vector3(0f, 179f, 0f), 1f).SetEase(Ease.InOutQuad))
            .Append(transform.DOMoveZ(-31.25f, 2f).SetEase(Ease.Linear))
            .Append(transform.DORotate(new Vector3(0f, 269f, 0f), 1f).SetEase(Ease.InOutQuad))
            .Append(transform.DOMoveX(27f, 5f).SetEase(Ease.Linear))
            .Append(transform.DORotate(new Vector3(0f, 224f, 0f), 1f).SetEase(Ease.InOutQuad))
            .Append(transform.DORotate(new Vector3(0f, 299f, 0f), 2f).SetEase(Ease.InOutQuad))
            .Append(transform.DORotate(new Vector3(0f, 419f, 0f), 1f).SetEase(Ease.InOutQuad).SetDelay(3f))
            .Append(transform.DOMoveX(31f, 2f).SetEase(Ease.Linear))
            .Join(transform.DORotate(new Vector3(0f, 1f, 0f), 2f).SetEase(Ease.InQuad))
            .OnComplete(nachTV.ActivateIntro);
    }

    // When Carey yells "HELLO?!" during his intro
    private IEnumerator CareyCallout(TV nachTV)
    {

        yield return new WaitForSeconds(0f);
        FindObjectOfType<AudioSource>().PlayOneShot(introMusic);

        yield return new WaitForSeconds(12f);
        nachTV.TurnOn();
        FindObjectOfType<AudioSource>().PlayOneShot(careyCallout);
    }


}
