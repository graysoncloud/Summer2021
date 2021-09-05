using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BenefitValue : MonoBehaviour
{
    private int benefitValue = 0;

    //private int negativeConnectionIncrement = 1;

    private TMPro.TextMeshProUGUI benefitValueText;

    private void Start()
    {
        benefitValueText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        benefitValue = 0;
    }

    public void UpdateBenefitValue(int benefit)
    {
        benefitValue += benefit;
        benefitValueText.text = benefitValue.ToString();
        DrugManager.instance.desiredChems += benefit; //terrible way of doing this
        ContractDisplayer.instance.UpdateEffect(DrugManager.instance.desiredChems);
    }
}
