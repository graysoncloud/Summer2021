using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dream Event", menuName = "Dream Event", order = 1)]
public class DreamEvent : ScriptableEvent
{
    public enum dreamChar { MariaNeutral, MariaSmirk, WesleySmirk, WesleyLaugh}

    public dreamChar toFade;
    public bool fadeOut;
}
