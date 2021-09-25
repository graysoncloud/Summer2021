using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEvent : ScriptableEvent
{
    public enum dreamChar { MariaNeutral, MariaSmirk, WesleySmirk, WesleyLaugh}

    public dreamChar toFade;
    public bool fadeOut;
}
