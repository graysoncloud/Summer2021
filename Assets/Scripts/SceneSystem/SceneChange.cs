using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public SceneName newScene;
    public CharacterInstantiation[] characters;
    public TransitionStyle transitionStyle;

    public float predelay;
    public float postdelay;

    public bool increaseDay;

    // Typically used mid conversaiton. However, you could theoretically start a dialogue event by including a nextEvent here.
    public GameObject nextEvent;

    // These must be named exactly how the scenes are in the editor
    public enum SceneName {MorningRoutineScene, OfficeScene, DrugGameScene, Recap}

    public enum TransitionStyle { instant, fade}

    [System.Serializable]
    public class CharacterInstantiation
    {
        public CharacterName character;
        public Vector2 location;
        public AnimationName animation;
    }
}
