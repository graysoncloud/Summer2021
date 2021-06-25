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
    string prevDialogueText = "";

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GameObject textObject = playerData as GameObject;
        string currentCharacterText = "";
        string currentDialogueText = "";
       
        Vector2 currentOffset = new Vector2();
        float currentAlpha = 0f;

        if (!textObject) return;

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if (inputWeight > 0f)
            {
                ScriptPlayable<DialogueBehavior> inputPlayable = (ScriptPlayable<DialogueBehavior>)playable.GetInput(i);

                DialogueBehavior input = inputPlayable.GetBehaviour();
                currentCharacterText = input.currentCharacterText;
                currentDialogueText = input.currentDialogueText;
                Debug.Log(prevDialogueText + ", " + currentDialogueText);
                currentOffset = input.currentOffset;
                currentAlpha = inputWeight;
            }
        }

        if (prevDialogueText != currentDialogueText)
        {
            // Text object itself doesn't have a text component- it merely houses everything else
            textObject.gameObject.transform.position = new Vector3(currentOffset.x, currentOffset.y - 3.25f, 0);

            TextMeshProUGUI characterText = textObject.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>();
            characterText.text = currentCharacterText;

            DialogueText dialogueText = textObject.transform.Find("DialogueText").GetComponent<DialogueText>();
            //dialogueText.GetComponent<TextMeshProUGUI>().text = currentDialogueText;
            dialogueText.DisplayText(currentDialogueText);
            dialogueText.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, currentAlpha);
        }

        prevDialogueText = currentDialogueText;

    }
}
