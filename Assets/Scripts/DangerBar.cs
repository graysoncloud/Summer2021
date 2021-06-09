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
    private TextMeshProUGUI dangerText;
    [SerializeField]
    private SpriteRenderer dangerBar;

    private int unstableIncrement = 4;
    private int negativeConnectionIncrement = 1;


    private void Start()
    {
        dangerValue = 0;
    }

    // Takes in the 12 statuses that were possibly altered from a rotation, placement or pickup, and adjusts the danger level accordingly
    public void UpdateDanger(string[] oldStatuses, string[] newStatuses)
    {
        for (int i = 0; i < oldStatuses.Length; i ++)
        {
            // expand if else statements to increase separate variables to display different bars, i.e. unstable amount vs. negative amount
            if (newStatuses[i] == "Unstable")
                dangerValue += 4;
            else if (newStatuses[i] == "Negative")
                dangerValue += 1;

            // This essentially reverses the above affect if nothing changed
            if (oldStatuses[i] == "Unstable")
                dangerValue -= 4;
            else if (oldStatuses[i] == "Negative")
                dangerValue -= 1;
        }

        dangerText.text = dangerValue.ToString();
        dangerBar.transform.localScale = new Vector3(dangerValue * 4, dangerBar.transform.localScale.y, dangerBar.transform.localScale.z);
    }
}
