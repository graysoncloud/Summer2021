using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DangerBar : MonoBehaviour
{
    [SerializeField]
    private int dangerValue;

    // Will probably be replaced by bar apparatus
    [SerializeField]
    private TextMeshProUGUI dangerText = null;
    [SerializeField]
    private SpriteRenderer dangerBar = null, dangerBG = null;
    [SerializeField]
    private int unstableVol = 5, negativeVol = 3, positiveVol = -2;

    //private int unstableIncrement = 4;
    //private int negativeConnectionIncrement = 1;


    private void Start()
    {
        dangerValue = 0;
    }

    // Takes in the 12 statuses that were possibly altered from a rotation, placement or pickup, and adjusts the danger level accordingly
    public void UpdateDanger(string[] oldStatuses, string[] newStatuses)
    {
        for (int i = 0; i < oldStatuses.Length; i++)
        {
            // expand if else statements to increase separate variables to display different bars, i.e. unstable amount vs. negative amount
            if (newStatuses[i] == "Unstable")
                dangerValue += unstableVol;
            else if (newStatuses[i] == "Negative")
                dangerValue += negativeVol;
            else if (newStatuses[i] == "Positive")
                dangerValue += positiveVol;


            // This essentially reverses the above affect if nothing changed
            if (oldStatuses[i] == "Unstable")
                dangerValue -= unstableVol;
            else if (oldStatuses[i] == "Negative")
                dangerValue -= negativeVol;
            else if (oldStatuses[i] == "Positive")
                dangerValue -= positiveVol;
        }

        dangerText.text = dangerValue.ToString();
        if (dangerValue < 0)
        {
            dangerBar.transform.localScale = new Vector3(0, dangerBar.transform.localScale.y, dangerBar.transform.localScale.z);
        } else if (dangerValue * 4 < dangerBG.transform.localScale.x * dangerBG.size.x)
        {
            dangerBar.transform.localScale = new Vector3(dangerValue * 4, dangerBar.transform.localScale.y, dangerBar.transform.localScale.z);
        } else
        {
            dangerBar.transform.localScale = new Vector3(dangerBG.transform.localScale.x * dangerBG.size.x, dangerBar.transform.localScale.y, dangerBar.transform.localScale.z);
        }
        
    }

    public int GetVol()
    {
        return dangerValue;
    }
}
