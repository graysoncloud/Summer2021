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

        public enum CharacterName { Barney, Elizabeth, Wesley, Robbie, Maria }

        [System.Serializable]
        public class AnimationMoment
        {
            public enum AnimationName { Walk, Wave, Sit }

            public CharacterName toAnimate;
            public AnimationName animationName;
            // Time before executing the animation
            public float delay;
            // Determines if sprite needs to be moved as a part of the animation, and if so, how fast / where it should go
            public bool requiresMovement;
            // Perhaps use a very large int to specify if x / y shouldn't change
            public Vector2 endLocation;
            public float moveSpeed;
        }
    }
}
