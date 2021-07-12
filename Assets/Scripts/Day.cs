using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Day : MonoBehaviour
{
    public Sequence[] sequences;
    public Contract[] contracts;

    [System.Serializable]
    public class Sequence
    {
        // Could be a scene change, conversation, etc.
        public GameObject initialEvent;
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
