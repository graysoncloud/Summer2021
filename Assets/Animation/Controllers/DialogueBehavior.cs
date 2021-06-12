using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

// This class specifies how a dialogue track is supposed to be read, and stores relevant variables for the current clip.
//      However, its functionality is minimal because dialogueTrackMixer handles most of what this would do

public class DialogueBehavior : PlayableBehaviour
{
    public string currentCharacterText;
    public string currentDialogueText;
    public Vector2 currentOffset;
}
