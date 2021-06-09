using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// The individual clips used within the timeline, which are passed into and update the DialogueBehavior class

public class DialogueClip : PlayableAsset
{
    public string dialogueText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehavior>.Create(graph);

        DialogueBehavior dialogueBehavior = playable.GetBehaviour();
        dialogueBehavior.currentDialogueText = dialogueText;

        return playable;
    }
}
