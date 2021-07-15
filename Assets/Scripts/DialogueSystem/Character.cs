using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int attitude;

    private void Start()
    {
        attitude = 100;

        // Add something to set attitude equal to save file state
    }

}

public enum CharacterName { 
    Barney, 
    Elizabeth, 
    Wesley, 
    Robbie, 
    Maria,
    Lumberjack,
    Purplejack
}
