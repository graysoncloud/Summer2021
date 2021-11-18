using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color highlightedColor;    
    [SerializeField]
    private Color disabledColor;

    [SerializeField]
    [Range(0f, 10f)]
    private float fadeTime;

    private Coroutine highlightCoroutine;
    private Coroutine dehighlightCoroutine;

    public void OnMouseDown()
    {
        if(GetComponent<EventTrigger>().enabled != false && GameManager.instance.sequenceActive == false)
        {
            DehighlightText();
            SFXPlayer.instance.PlaySoundEffect(13);
        }
    }

    private void OnMouseEnter()
    {
        if (GetComponent<EventTrigger>().enabled != false && GameManager.instance.sequenceActive == false)
        {
            SFXPlayer.instance.PlaySoundEffect(12);
        }
    }

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
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponent<TextMeshProUGUI>().color, highlightedColor, fadeTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator Dehighlight()
    {
        while (gameObject.GetComponent<TextMeshProUGUI>().color != defaultColor)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponent<TextMeshProUGUI>().color, defaultColor, fadeTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

    }

    public Color GetDefaultColor()
    {
        return defaultColor;
    }

    public void Disable()
    {
        GetComponent<EventTrigger>().enabled = false;
        gameObject.GetComponent<TextMeshProUGUI>().color = disabledColor;

    }

    public void Enable()
    {
        GetComponent<EventTrigger>().enabled = true;
        gameObject.GetComponent<TextMeshProUGUI>().color = defaultColor;

    }


}
