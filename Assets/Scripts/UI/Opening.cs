using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opening : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0, endSpeed = 0, logoDelay, delay = 0, stopDelay, endDelay, start2Delay, stop2Delay;
    [SerializeField]
    private TextMeshProUGUI title, thankYou;
    [SerializeField]
    private Image logo, lmuLogo;

    [SerializeField]
    private SceneChange toTitleScreen;

    [SerializeField]
    private Animator animator; 

    private bool start = false, end = false, start2 = false, end2 = false, stop = false;

    private void Awake()
    {
        title.alpha = 0;
        animator.enabled = false;
    }

    void Start()
    {
        StartCoroutine(LogoStart());
        StartCoroutine(Begin());
        StartCoroutine(End());
        StartCoroutine(Begin2());
        StartCoroutine(End2());
        StartCoroutine(Stop());
    }

    IEnumerator LogoStart()
    {
        yield return new WaitForSeconds(logoDelay);
        animator.enabled = true;
    }

    IEnumerator Begin()
    {
        SFXPlayer.instance.PlaySoundEffect(11);
        yield return new WaitForSeconds(delay);
        start = true;
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(stopDelay);
        start = false;
        end = true;
    }

    IEnumerator Begin2()
    {
        yield return new WaitForSeconds(start2Delay);
        start2 = true;
    }

    IEnumerator End2()
    {
        yield return new WaitForSeconds(stop2Delay);
        start2 = false;
        end2 = true;
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(endDelay);
        end = false;
        stop = true;
    }

    private void FixedUpdate()
    {
        if (start)
        {
            title.alpha += fadeSpeed / 100;
        }
        if (end)
        {
            title.alpha -= endSpeed / 100;
            logo.color = new Color (logo.color.r, logo.color.g, logo.color.b, logo.color.a - endSpeed / 150);
        }
        if (start2)
        {
            lmuLogo.color = new Color(lmuLogo.color.r, lmuLogo.color.g, lmuLogo.color.b, lmuLogo.color.a + fadeSpeed / 150);
            thankYou.color = new Color(lmuLogo.color.r, lmuLogo.color.g, lmuLogo.color.b, lmuLogo.color.a + fadeSpeed / 150);

        }
        if (end2)
        {
            lmuLogo.color = new Color(lmuLogo.color.r, lmuLogo.color.g, lmuLogo.color.b, lmuLogo.color.a - endSpeed / 150);
            thankYou.color = new Color(lmuLogo.color.r, lmuLogo.color.g, lmuLogo.color.b, lmuLogo.color.a - endSpeed / 150);

        }
        if (stop)
        {
            //Debug.Log("Stop called");

            SceneChangeManager.instance.StartSceneChange(toTitleScreen);
            gameObject.SetActive(false);
        }
    }
}
