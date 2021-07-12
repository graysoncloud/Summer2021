using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public DialogueLine[] dialogueLines;
    public ScriptableObject nextEvent;

    [System.Serializable]
    public class DialogueLine
    {
        // If you create a character object, speaking character should be a reference to those
        public CharacterName speaker;
        [TextArea]
        public string dialogue;
        public Vector2 offset;

    }
}
