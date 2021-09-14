using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MigraineEvent", menuName = "MigraineEvent", order = 1)]
public class MigraineEvent : ScriptableEvent
{
    public int pulseNum = 3;
    public float strength = 100;
}
