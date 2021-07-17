using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public static ConversationManager instance = null;

    public Conversation currentConversation;
    // While "inConversation", certain objects won't respond to clicks and whatnot
    public bool inConversation;
    private bool clicked;
    private bool waitingForClick;

    // Serves as a indicator of whether or not the conversation can continue (all animations must finish before clicking past a dialogue line)
    private int activeAnimations;

    private float periodDelay = .33f;
    private float punctuationDelay = .2f;
    private float normalDelay = .066f;
    private float readbackSpeedModifier = 1;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Update()
    {
        // The "clicked" boolean is necessary because OnMouseButtonDown doesn't consistently work within Coroutines
        if (Input.GetMouseButtonDown(0) && inConversation && waitingForClick)
            clicked = true;
            // clicked is only turned off once its "uses", so if my logic is wrong, it could get stuck into on
    }

    /**
     * Can be called from any class to begin a specified conversation.
     */
    public void StartConversation(Conversation conversation)
    {
        // May be redundant but there's not really a need for a second inConversation variable
        DialogueUIManager.instance.SetUpForConversation();

        if (!inConversation)
        {
            // These things will already be on if mid-conversation.
            inConversation = true;
            // Just turn on what needs to be turned on
            DialogueUIManager.instance.SetUpForConversation();
        }

        StartCoroutine(ExecuteConversation(conversation));
    }

    /**
     * Handles logic / flow of conversation
     */
    private IEnumerator ExecuteConversation(Conversation conversation)
    {
        // Could be a redundant variable, but it could be useful (not a big deal either way)
        currentConversation = conversation;
        for (int i = 0; i < currentConversation.dialogueLines.Length; i++)
        {
            DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters = 0;

            // Reveal characterName, and then set up dialogue text (but make it invisible)
            DialogueUIManager.instance.characterTextObject.text = currentConversation.dialogueLines[i].speaker.ToString();
            DialogueUIManager.instance.dialogueTextObject.text = currentConversation.dialogueLines[i].dialogue;


            // Adjust character animations
            foreach (Conversation.AnimationBit anim in conversation.dialogueLines[i].animations)
            {
                //Character toAnimate = CharacterFadeManager.instance.currentChars[anim.toAnimate.ToString()];

                //foreach (AnimationClip animClip in toAnimate.GetComponent<Animator>().runtimeAnimatorController.animationClips)
                //{
                //    Debug.Log(animClip.name);
                //    Debug.Log(anim.animationName.ToString());
                //    Debug.Log(animClip.name == anim.animationName.ToString());

                //    if (animClip.name == anim.animationName.ToString()) {
                //        toAnimate.GetComponent<Animator>().Play(animClip.name);
                //    }

                //}

                Character toAnimate = CharacterFadeManager.instance.currentChars[anim.toAnimate.ToString()];
                toAnimate.GetComponent<Animator>().Play(currentConversation.dialogueLines[i].animations[0].animationName.ToString());


            }


            // Reveals dialogue one letter at a time; fastforward on first click, instantly finish on second
            waitingForClick = true;
            bool readbackSpeedChanged = false;
            foreach (char toReveal in currentConversation.dialogueLines[i].dialogue)
            {
                DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters += 1;

                // Fastforward on first click
                if (clicked && !readbackSpeedChanged)
                {
                    // Commented code instantly reveals the text; current version speeds it up by 10
                    //DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters = currentConversation.dialogueLines[i].dialogue.Length;
                    readbackSpeedModifier /= 7;
                    readbackSpeedChanged = true;
                    clicked = false;
                    // Instantly finish on second click
                } else if (clicked && readbackSpeedChanged)
                {
                    DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters = DialogueUIManager.instance.dialogueTextObject.text.Length;
                    clicked = false;
                    break;
                }

                // Delay between characters increases if its a punctuation mark
                if (toReveal == '.')
                    yield return new WaitForSeconds(periodDelay * readbackSpeedModifier);
                else if (toReveal == ' ')
                    yield return new WaitForSeconds(0);
                else if (",;-?!".Contains(toReveal.ToString()))
                    yield return new WaitForSeconds(punctuationDelay * readbackSpeedModifier);
                else
                    yield return new WaitForSeconds(normalDelay * readbackSpeedModifier);
            }

            waitingForClick = false;
            // This gives a small buffer to prevent player from double clicking
            yield return new WaitForSeconds(.1f);
            // Makes sure the user doesn't "store" a click during the .25 seconds of waiting
            clicked = false;

            readbackSpeedModifier = 1;
            // Wait for player to click to continue
            yield return StartCoroutine("WaitForClick");

            DialogueUIManager.instance.dialogueTextObject.text = "";
            DialogueUIManager.instance.characterTextObject.text = "";
        }

        if (currentConversation.nextEvent == null)
            EndConversation();
        else
            GameManager.instance.StartSequence(currentConversation.nextEvent);

        // Don't need to destroy because we're just using the prefab without instantiating
        //Destroy(currentConversation);
    }

    private IEnumerator WaitForClick()
    {
        waitingForClick = true;
        while (!clicked)
        {
            yield return new WaitForEndOfFrame();
        }
        clicked = false;
        waitingForClick = false;
    }

    public void EndConversation()
    {
        // Might be better to handle the record keeping in a GameManager- a better name for inConversation might be inDialogueScene
        waitingForClick = false;
        readbackSpeedModifier = 1;
        inConversation = false;
        DialogueUIManager.instance.turnOffDialogueUI();
    }

    public void JumpToSceneChange()
    {

        ScriptableEvent currentEvent = currentConversation;
        int breaker = 0;

        while (currentEvent.nextEvent != null)
        {
            //Debug.Log(currentEvent.GetType().ToString());

            breaker++;
            if (breaker > 10)
                break;

            currentEvent = currentEvent.nextEvent;

            Option nextEventAsOption;
            if (currentEvent.GetType().ToString() == "Option")
            {
                nextEventAsOption = (Option)currentEvent;
                currentEvent = nextEventAsOption.possibleConversations[0];
            }



            if (currentEvent.GetType().ToString() == "SceneChange")
            {
                SceneChangeManager.instance.StartSceneChange((SceneChange)currentEvent);
                return;
            }
        }
    }


}
