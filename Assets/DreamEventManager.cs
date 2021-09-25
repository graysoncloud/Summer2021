using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEventManager : MonoBehaviour
{
    public static DreamEventManager instance;

    private Color colorIncrement = new Color(0f, 0f, 0f, .015f);

    // Needs to be laid out exactly as the dreamChar enum is
    public GameObject[] dreamChars;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void StartDreamEvent(DreamEvent dE)
    {
        StartCoroutine(ExecuteDreamEvent(dE));
    }

    IEnumerator ExecuteDreamEvent(DreamEvent dE)
    {
        SpriteRenderer toFade = dreamChars[(int)dE.toFade].GetComponent<SpriteRenderer>();

        if (!dE.fadeOut)
        {
            while (toFade.color.a < 1)
            {
                toFade.color += colorIncrement * 60 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        else
        {
            while (toFade.color.a > 0)
            {
                toFade.color -= colorIncrement * 60 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        GameManager.instance.StartSequence(dE.nextEvent);

    }


}
