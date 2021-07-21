using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "CharacterFade", menuName = "CharacterFade", order = 1)]
public class CharacterFade : ScriptableEvent
{
    public Fade[] characterFades;

    [System.Serializable]
    public class Fade
    {
        public CharacterName characterToFade;
        //public AnimationName animationName;
        public bool fadeIn;
        public CharacterFadeSFX[] SFX;

    }

}
