using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Day[] days;
    public int currentDay;

    private void Start()
    {
        currentDay = 0;

        // Should be an if statement to allow for saving
    }

}
