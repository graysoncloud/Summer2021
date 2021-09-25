using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitManager : MonoBehaviour
{
    public static WaitManager instance;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void StartWaitEvent(WaitEvent wE)
    {
        DialogueUIManager.instance.SetUpForWait();
        StartCoroutine(ExecuteWaitEvent(wE));
    }
    
    IEnumerator ExecuteWaitEvent(WaitEvent wE)
    {
        Debug.Log("Start");
        yield return new WaitForSeconds(wE.secondsToWait);
        Debug.Log("End");

        GameManager.instance.StartSequence(wE.nextEvent);
    }

}
