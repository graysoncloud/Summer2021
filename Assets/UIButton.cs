using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIButton : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color highlightedColor;

    [SerializeField]
    [Range(0f, 1f)]
    private float fadeTime;

    private Coroutine highlightCoroutine;
    private Coroutine dehighlightCoroutine;

    public void HighlightText()
    {
        if (dehighlightCoroutine != null)
            StopCoroutine(dehighlightCoroutine);

        highlightCoroutine = StartCoroutine("Highlight");

    }

    public void DehighlightText()
    {
        if (highlightCoroutine != null)
            StopCoroutine(highlightCoroutine);

        dehighlightCoroutine = StartCoroutine("Dehighlight");

    }

    IEnumerator Highlight()
    {
        while (gameObject.GetComponent<TextMeshProUGUI>().color != highlightedColor)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponent<TextMeshProUGUI>().color, highlightedColor, fadeTime);
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator Dehighlight()
    {
        while (gameObject.GetComponent<TextMeshProUGUI>().color != defaultColor)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponent<TextMeshProUGUI>().color, defaultColor, fadeTime);
            yield return new WaitForEndOfFrame();
        }

    }

    public Color GetDefaultColor()
    {
        return defaultColor;
    }


}
