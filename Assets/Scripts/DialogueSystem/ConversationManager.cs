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

    private float periodDelay = .5f;
    private float punctuationDelay = .3f;
    private float normalDelay = .1f;

    // Its a little sloppy to store references to every needed conversation, but I couldn't find a better way
    // Perhaps a conversationStorage object would be useful to avoid cluttering the GameManager
    // (this is just for test purposes, I don't think Conversations should be stored in this class)
    public Conversation toExecute;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitiateConversation(toExecute);
    }

    private void Update()
    {
        // The "clicked" boolean is necessary because OnMouseButtonDown doesn't consistently work within Coroutines
        if (Input.GetMouseButtonDown(0) && inConversation)
            clicked = true;
        // clicked is only turned off once its "uses", so if my logic is wrong, it could get stuck into on
    }

    /**
     * Can be called from any class to begin a specified conversation.
     */
    public void InitiateConversation(Conversation conversation)
    {
        clicked = false;
        if (!inConversation)
        {
            // These things will already be on if mid-conversation.
            inConversation = true;
            // Just turn on what needs to be turned on
            DialogueUIManager.instance.EnableDialogueOverlay();
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
            //Debug.Log(currentConversation.dialogueLines[i].dialogue);
            DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters = 0;
            // Reveal character, and then set up dialogue text (but make it invisible)
            DialogueUIManager.instance.characterTextObject.text = currentConversation.dialogueLines[i].speaker.ToString();
            DialogueUIManager.instance.dialogueTextObject.text = currentConversation.dialogueLines[i].dialogue;
            // Reveals dialogue one letter at a time; skips to end if mouse is clicked
            // Could change it to be such that characters are just revealed faster once you click
            foreach (char toReveal in currentConversation.dialogueLines[i].dialogue)
            {
                DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters += 1;
                if (clicked)
                {
                    DialogueUIManager.instance.dialogueTextObject.maxVisibleCharacters = currentConversation.dialogueLines[i].dialogue.Length;
                    clicked = false;
                    break;
                }
                // Delay between characters increases if its a punctuation mark
                if (toReveal == '.')
                    yield return new WaitForSeconds(periodDelay);
                else if (toReveal == ' ')
                    yield return new WaitForSeconds(0);
                else if (",;-?!".Contains(toReveal.ToString()))
                    yield return new WaitForSeconds(punctuationDelay);
                else
                    yield return new WaitForSeconds(normalDelay);
            }
            // Wait for player to click to continue
            yield return StartCoroutine("WaitForClick");
            DialogueUIManager.instance.dialogueTextObject.text = "";
            DialogueUIManager.instance.characterTextObject.text = "";
        }

        if (currentConversation.nextEvent == null)
            EndConversation();
        else if (currentConversation.nextEvent.GetComponent<Option>() != null)
            OptionManager.instance.PresentOption(currentConversation.nextEvent.GetComponent<Option>());
        else if (currentConversation.nextEvent.GetComponent<Conversation>() != null)
            ExecuteConversation(currentConversation.nextEvent.GetComponent<Conversation>());
        // Add an else if for scene transition
        else
            Debug.LogError("Invalid next event");

        // Don't need to destroy because we're just using the prefab without instantiating
        //Destroy(currentConversation);
    }

    private IEnumerator WaitForClick()
    {
        while (!clicked)
        {
            yield return new WaitForEndOfFrame();
        }
        clicked = false;
    }

    public void EndConversation()
    {
        inConversation = false;
        DialogueUIManager.instance.DisableDialogueOverlay();
    }


}
