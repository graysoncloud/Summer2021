using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opening : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0, endSpeed = 0, logoDelay, delay = 0, stopDelay, endDelay;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private Image logo;

    [SerializeField]
    private SceneChange toTitleScreen;

    [SerializeField]
    private Animator animator; 

    private bool start = false, end = false, stop = false;

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
        StartCoroutine(Stop());
    }

    IEnumerator LogoStart()
    {
        yield return new WaitForSeconds(logoDelay);
        animator.enabled = true;
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(delay);
        start = true;
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(stopDelay);
        start = false;
        end = true;
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
        if (stop)
        {
            //Debug.Log("Stop called");

            SceneChangeManager.instance.StartSceneChange(toTitleScreen);
            gameObject.SetActive(false);
        }
    }
}
