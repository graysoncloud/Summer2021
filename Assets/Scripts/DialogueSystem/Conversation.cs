using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation", order = 1)]
public class Conversation : ScriptableEvent
{

    public bool isNews;
    public DialogueLine[] dialogueLines;

    [System.Serializable]
    public class DialogueLine
    {
        // If you create a character object, speaking character should be a reference to those
        public CharacterName speaker;
        [TextArea(6, 20)]
        public string dialogue;
        public Vector2 offset;

        public AnimationBit[] animations;
        public bool animOnly;
        public float animLength;

        public ConversationSFX[] SFX;
    }

    [System.Serializable]
    public class AnimationBit
    {
        public CharacterName toAnimate;
        public AnimationName animationName;

    }

}
