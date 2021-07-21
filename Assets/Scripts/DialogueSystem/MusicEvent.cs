using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicEvent", menuName = "MusicEvent", order = 1)]
public class MusicEvent : ScriptableEvent
{

    public bool fadeOut;
    public Song toPlay;

}

public enum Song
{
    Work1,
    Work2,
    Work3,
    MorningTheme1,
    MorningTheme2,
    WesleyTheme,
    MariaTheme,
    RobbieTheme,
    ElizabethTheme,

}
