using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashingText : TextMeshProUGUI
{
    private Color toChangeBy = new Color(.01f, .01f, .01f, 0f);
    private bool ascending;

    protected override void Start()
    {
        ascending = true;
        color = new Color(.55f, .55f, .55f, 1f);
    }

    private void Update()
    {

        if (ascending)
        {
            color += toChangeBy * Time.deltaTime * 60;
            if (color.r >= 1)
                ascending = false;
        } else
        {
            color -= toChangeBy * Time.deltaTime * 60;
            if (color.r <= .55)
                ascending = true;
        }
    }

}
