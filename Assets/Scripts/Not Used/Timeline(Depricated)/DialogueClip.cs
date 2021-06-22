using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// The individual clips used within the timeline, which are passed into and update the DialogueBehavior class

public class DialogueClip : PlayableAsset
{
    // If you want names with spaces, just parse through the string looking for capitals and add a space before them
    public enum CharEnum { Barney, Larry };

    public string dialogueText;
    public Vector2 offset;
    public CharEnum character;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehavior>.Create(graph);

        DialogueBehavior dialogueBehavior = playable.GetBehaviour();
        dialogueBehavior.currentCharacterText = character.ToString();
        dialogueBehavior.currentDialogueText = dialogueText;
        dialogueBehavior.currentOffset = offset;

        return playable;
    }
}
