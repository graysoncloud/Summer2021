using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayButton : MonoBehaviour
{

    private void OnMouseDown()
    {

        foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
        {
            if (sequence.trigger.ToString() == "dream")
            {
                GameManager.instance.StartSequence(sequence.initialEvent);
                return;
            }
        }

        SceneChangeManager.instance.StartSceneChange(RecapSceneManager.instance.recapToMR);

        GameManager.instance.NextDay();
    }
}
