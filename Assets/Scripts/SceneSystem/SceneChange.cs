using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneChange", menuName = "SceneChange", order = 1)]
public class SceneChange : ScriptableEvent
{
    public SceneName newScene;
    //public CharacterInstantiation[] characters;
    public TransitionStyle transitionStyle;

    public float preDelay;
    public float midDelay;
    public float postDelay;

    public bool increaseDay;

    // These must be named exactly how the scenes are in the editor
    public enum SceneName { 
        MorningRoutineScene, 
        OfficeScene, 
        DrugGameScene, 
        RecapScene, 
        DreamScene,
        TitleScene,
        OSOverlay,
        OpeningScene,
        CreditsScene,
        OverBlackScene
    }

    public enum TransitionStyle { 
        instant, 
        fade,
        longFade,
    }

    [System.Serializable]
    public class CharacterInstantiation
    {
        public CharacterName character;
        public Vector2 location;
        public AnimationName animation;
    }
}
