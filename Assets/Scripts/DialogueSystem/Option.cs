using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    // The strings which will be displayed to the player
    [TextArea]
    public string[] options;

    // One conversations that will result from the options (must line up with the string array)
    public Conversation[] possibleConversations;
}
