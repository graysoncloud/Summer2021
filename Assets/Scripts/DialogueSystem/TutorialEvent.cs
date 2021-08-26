using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "TutorialEvent", menuName = "TutorialEvent", order = 1)]
public class TutorialEvent : ScriptableEvent
{

    // Note this doesn't actually work right now, it's only used once so it just defaults to the right tutorial
    public GameObject tutorial;


}
