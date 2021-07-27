using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningRoutineITem : MonoBehaviour
{
    bool shown = true;
    void Update()
    {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            gameObject.SetActive(false);
            shown = false;
        } else {
            if(shown == false) {
                 gameObject.SetActive(true);
            }
        }
    }
}
