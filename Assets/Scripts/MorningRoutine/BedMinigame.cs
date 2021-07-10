using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedMinigame : Minigame
{
    // Start is called before the first frame update
    new void Start()
    {
        FindObjectOfType<Blanket>().Reset();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementDay() {
        FindObjectOfType<Blanket>().Reset();
    }
}
