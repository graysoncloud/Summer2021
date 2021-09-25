using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElizabethHitbox : MonoBehaviour
{
    public static ElizabethHitbox instance;

    PolygonCollider2D collider2D;

    public int timesClickedToday;

    [SerializeField]
    public Conversation[] firstClickReactions;
    [SerializeField]
    public Conversation[] secondClickReactions;
    [SerializeField]
    public Conversation[] thirdClickReactions;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        timesClickedToday = 0;
    }


    private void OnMouseDown()
    {
        if (GameManager.instance.sequenceActive)
        {
            return;
        }

        timesClickedToday++;

        if (timesClickedToday == 1)
        {
            int rInt = Random.Range(0, firstClickReactions.Length);
            ConversationManager.instance.StartConversation(firstClickReactions[rInt]);
        } else if (timesClickedToday == 2)
        {
            int rInt = Random.Range(0, secondClickReactions.Length);
            ConversationManager.instance.StartConversation(secondClickReactions[rInt]);
        } else
        {
            int rInt = Random.Range(0, thirdClickReactions.Length);
            ConversationManager.instance.StartConversation(thirdClickReactions[rInt]);
        }

    }

}
