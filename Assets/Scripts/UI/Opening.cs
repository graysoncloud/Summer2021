using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opening : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0, endSpeed = 0, delay = 0, stopDelay, endDelay;
    [SerializeField]
    private TextMeshProUGUI title;

    private bool start = false, end = false, stop = false;

    private void Awake()
    {
        title.alpha = 0;
    }

    void Start()
    {
        StartCoroutine(Begin());
        StartCoroutine(End());
        StartCoroutine(Stop());
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
        }
        if (stop)
        {
            Debug.Log("Stop called");
            //Switch scenes
        }
    }
}
