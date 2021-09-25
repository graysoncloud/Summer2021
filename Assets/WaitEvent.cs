using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "Wait", order = 1)]
public class WaitEvent : ScriptableEvent
{
    public float secondsToWait;

}
