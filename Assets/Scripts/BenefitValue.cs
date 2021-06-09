using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenefitValue : MonoBehaviour
{
    [SerializeField]
    private int benefitValue;

    // Will probably be replaced by bar apparatus
    [SerializeField]
    private TextMeshProUGUI benefitValueText;

    private int negativeConnectionIncrement = 1;


    private void Start()
    {
        benefitValue = 0;
    }

    // Takes in the 12 statuses that were possibly altered from a rotation, placement or pickup, and adjusts the danger level accordingly
    public void UpdateBenefitValue(string[] oldStatuses, string[] newStatuses)
    {
        for (int i = 0; i < oldStatuses.Length; i++)
        {
            if (newStatuses[i] == "Positive")
                benefitValue += 1;

            // This essentially reverses the above affect if nothing changed
            if (oldStatuses[i] == "Positive")
                benefitValue -= 1;
        }

        benefitValueText.text = benefitValue.ToString();
    }
}
