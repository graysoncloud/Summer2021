using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Day : MonoBehaviour
{
    public DialogueEvent[] dialogueEvents;
    public Contract[] contracts;

    [System.Serializable]
    public class DialogueEvent
    {
        // Could be a scene change, conversation, etc.
        public GameObject startingEvent;
        public Trigger trigger;

        public enum Trigger
        {
            wakingUp,
            leavingHome,
            arrivingAtWork,
            solvedContract1,
            solvedContract2,
            solvedContract3,
            solvedContract4,
            solvedContract5,
            solvedContract6,
            arrivingHome,
        }

    }

}
