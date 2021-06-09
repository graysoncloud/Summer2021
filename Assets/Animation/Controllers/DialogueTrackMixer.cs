using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

// Where the dialogue clip information is processed and translated into in-game effects
// The base functionality here could be accomplished in the dialogue behavior class, but this class allows us to do certain things like
//     doing "nothing" on empty frames (rather than keeping up the old dialogue)

public class DialogueTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI textObject = playerData as TextMeshProUGUI;
        string currentText = "";
        float currentAlpha = 0f;

        if (!textObject) return;

        int inputCount = playable.GetInputCount();
        Debug.Log(inputCount);
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if (inputWeight > 0f)
            {
                ScriptPlayable<DialogueBehavior> inputPlayable = (ScriptPlayable<DialogueBehavior>)playable.GetInput(i);

                DialogueBehavior input = inputPlayable.GetBehaviour();
                currentText = input.currentDialogueText;
                currentAlpha = inputWeight;
            }
        }

        textObject.text = currentText;
        textObject.color = new Color(0, 0, 0, currentAlpha);
    }
}
