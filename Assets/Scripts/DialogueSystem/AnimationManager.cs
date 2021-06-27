 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance = null;

    // Serves as a indicator of whether or not the conversation can continue (all animations must finish before clicking past a dialogue line)
    public int activeAnimations;

    public GameObject characterPool;

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
        activeAnimations = 0;
    }

    public void StartAnimationMoment(AnimationMoment anim)
    {
        StartCoroutine(ExecuteAnimation(anim));
    }

    private IEnumerator ExecuteAnimation(AnimationMoment anim)
    {
        activeAnimations++;
        Character charToAnimate = SceneChangeManager.instance.currentScene.transform.Find(anim.charName.ToString()).gameObject.GetComponent<Character>();

        // Removes text boxes
        DialogueUIManager.instance.SetUpForAnimation();

        // Wait for a specified amount of time
        yield return new WaitForSeconds(anim.predelay);

        // This relies on the animationName exactly matching the name of the animation.
        // Also, animations should all be named generically- "Run", "Walk"
        charToAnimate.GetComponent<Animator>().Play(anim.animationName.ToString());

        // Moves the character towards a point 
        if (anim.requiresMovement)
            StartCoroutine(MoveCharacter(charToAnimate, anim.endLocation, anim.moveSpeed, anim.pauseWhileMoving));

        // The "duration" waiting occurs WHILE the character moves. It probably best to use either duration or pauseWhileMoving, but not both.
        // If it makes sense, add a "pauseAfterMoving" variable for a bit more control.
        yield return new WaitForSeconds(anim.duration);

        activeAnimations--;

        while (activeAnimations != 0)
            yield return new WaitForEndOfFrame();

        // This whole block could be a GameManager thing, or at least a static function
        if (anim.nextEvent == null)
            ConversationManager.instance.EndConversation();
        else if (anim.nextEvent.GetComponent<Conversation>() != null)
            ConversationManager.instance.StartConversation(anim.nextEvent.GetComponent<Conversation>());
        else if (anim.nextEvent.GetComponent<Option>() != null)
            OptionManager.instance.PresentOption(anim.nextEvent.GetComponent<Option>());
        else if (anim.nextEvent.GetComponent<AnimationMoment>() != null)
            AnimationManager.instance.StartAnimationMoment(anim.nextEvent.GetComponent<AnimationMoment>());
        else if (anim.nextEvent.GetComponent<SceneChange>() != null)
            SceneChangeManager.instance.StartSceneChange(anim.nextEvent.GetComponent<SceneChange>());
        else
            Debug.LogError("Invalid next event");
    }

    private IEnumerator MoveCharacter(Character toMove, Vector2 targetLocationVec2, float speed, bool pauseWhileMoving)
    {
        // Not technically an animation, but this makes it so that the next event in a dialogue sequence won't trigger while something's moving
        if (pauseWhileMoving)
            activeAnimations++;

        // Could alternatively code in acceleration to make it look cooler
        Vector3 targetLocation = new Vector3(targetLocationVec2.x, targetLocationVec2.y, toMove.transform.position.z);
        while (toMove.transform.position != targetLocation)
        {
            float stepSize = speed * Time.deltaTime * 2;
            toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, targetLocation, stepSize);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Ended pause while move");
        if (pauseWhileMoving)
            activeAnimations--;

    }

}
