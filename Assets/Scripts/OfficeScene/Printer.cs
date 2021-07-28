using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    public static Printer instance = null;

    public Sprite noSolutionSprite;
    public Sprite printedSolutionSprite;

    public bool solutionPrinted;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}
