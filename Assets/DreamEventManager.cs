using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEventManager : MonoBehaviour
{
    public static DreamEventManager instance;

    private Color colorIncrement = new Color(0f, 0f, 0f, .015f);

    // Needs to be laid out exactly as the dreamChar enum is
    public GameObject[] dreamChars;

    [SerializeField]
    public GameObject props;


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
        DialogueUIManager.instance.SetUpForDream();

        DreamCharacter character = dreamChars[(int)dE.toFade].GetComponent<DreamCharacter>();

        SpriteRenderer toFade = dreamChars[(int)dE.toFade].GetComponent<SpriteRenderer>();

        if (!dE.fadeOut)
        {
            dreamChars[(int)dE.toFade].GetComponent<DreamCharacter>().activated = true;

            while (toFade.color.a < 1)
            {
                toFade.color += colorIncrement * 60 * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        else
        {
            dreamChars[(int)dE.toFade].GetComponent<DreamCharacter>().activated = false;

            while (toFade.color.a > 0)
            {

                toFade.color -= colorIncrement * 60 * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            } 
        }

        GameManager.instance.StartSequence(dE.nextEvent);

    }

    public void ResetDreamScene()
    {
        foreach (SpriteRenderer sR in props.GetComponentsInChildren<SpriteRenderer>())
        {
            Destroy(sR.gameObject);
        }
    }


}
