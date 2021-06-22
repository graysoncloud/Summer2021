using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueText : MonoBehaviour
{
    public static DialogueText instance = null;
    private Coroutine currentDisplayCoroutine = null;

    [SerializeField]
    TextMeshProUGUI textField;

    private float normalDelay = .1f;
    private float punctuationDelay = .3f;
    private float periodDelay = .6f;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void DisplayText(string toDisplay)
    {
        Debug.Log(currentDisplayCoroutine == null);

        // Is it a problem that there's no code stopping multiple coroutines from running? Or is there?
        if (currentDisplayCoroutine != null)
            StopCoroutine(currentDisplayCoroutine);

        currentDisplayCoroutine = StartCoroutine(IncrementalDisplay(toDisplay));
    }

    private IEnumerator IncrementalDisplay(string dialogueText)
    {
        textField.text = "";

        for (int i = 0; i < dialogueText.Length; i++)
        {
            string toAdd = dialogueText.Substring(i, 1);
            textField.text += toAdd;

            if (toAdd == ".")
                yield return new WaitForSeconds(periodDelay);
            else if (",;-?!".Contains(toAdd))
                yield return new WaitForSeconds(punctuationDelay);
            else
                yield return new WaitForSeconds(normalDelay);
        }
    }
}
