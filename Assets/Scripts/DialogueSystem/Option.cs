using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Option", menuName = "Option", order = 1)]
public class Option : ScriptableEvent
{
    public bool automatic;

    // The strings which will be displayed to the player
    public Path[] paths;

    // One conversations that will result from the options (must line up with the string array)
    public Conversation[] possibleConversations;

}




