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
    Title,
    MR1,
    MR2,
    Work1,
    Work2,
    Work3,
    Work4,
    Work5,
    WesleyTheme,
    MariaTheme,
    RobbieTheme,
    ElizabethTheme,
}
