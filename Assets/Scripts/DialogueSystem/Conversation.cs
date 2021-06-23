using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public DialogueLine[] dialogueLines;
    public GameObject nextEvent;

    [System.Serializable]
    public class DialogueLine
    {
        // If you create a character object, speaking character should be a reference to those
        public CharacterName speaker;
        [TextArea]
        public string dialogue;
        public Vector2 offset;
        public AnimationMoment[] animations;

        public enum CharacterName { Barney, Elizabeth, Wesley, Robbie, Maria}

        [System.Serializable]
        public class AnimationMoment
        {
            public enum AnimationName { Walk, Wave, Sit }

            public Character toAnimate;
            public AnimationName animationName;
            public float Delay;
        }
    }

}
