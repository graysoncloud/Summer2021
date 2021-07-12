using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Day", order = 1)]
public class Day : ScriptableObject
{
    public Sequence[] sequences;
    public Contract[] contracts;

    [System.Serializable]
    public class Sequence
    {
        // Could be a scene change, conversation, etc.
        public ScriptableObject initialEvent;
        public Trigger trigger;

        public enum Trigger
        {
            leavingHome,
            solvedContract1,
            solvedContract2,
            solvedContract3,
            solvedContract4,
            solvedContract5,
            solvedContract6,
            leavingWork,
            dream,
            // Since going to bed and waking up is one combined sequence, waking up events must be specified the day before
            wakingUpNextDay,
        }

    }

}
