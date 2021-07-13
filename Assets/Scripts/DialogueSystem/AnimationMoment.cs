using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AnimationMoment", menuName = "AnimationMoment", order = 1)]
public class AnimationMoment : ScriptableEvent
{
    public CharacterName charName;
    public AnimationName animationName;

    // Time before executing the animation
    public float predelay;
    // Amount of time the AnimationManager waits before declaring that the animation moment has ended
    public float duration;

    // Determines if sprite needs to be moved as a part of the animation, and if so, how fast / where it should go
    public bool requiresMovement;

    // Perhaps use a very large int to specify if x / y shouldn't change
    public Vector2 endLocation;
    public float moveSpeed;
    public bool pauseWhileMoving;

    // Might add a bool that specifies if you should wait for a click after animation is done (end of scenes)
}

public enum AnimationName { Walk, Wave, Sit, Run, Idle, Standing}