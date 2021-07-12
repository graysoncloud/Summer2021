using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractDisplayer : MonoBehaviour
{
    public static ContractDisplayer instance = null;

    [SerializeField]
    private TextMeshProUGUI companyName;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private TextMeshProUGUI[] requirements;
    [SerializeField]
    private TextMeshProUGUI[] values;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void DisplayContract(int contractIndex)
    {
        // Erase the old data
        companyName.text = "";
        description.text = "";
        foreach (TextMeshProUGUI tmpro in requirements)
            tmpro.text = "";
        foreach (TextMeshProUGUI tmpro in values)
            tmpro.text = "";


        ContractSO contractToDisplay = GameManager.instance.currentDay.contracts[contractIndex];

        companyName.text = contractToDisplay.companyName;
        description.text = contractToDisplay.description;

        int currentDisplayItemIndex = 0;

        if (contractToDisplay.usesMaxVolatility)
        {
            requirements[currentDisplayItemIndex].text = "Max Volatility";
            values[currentDisplayItemIndex].text = contractToDisplay.volatilityMax.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesMaxPrice)
        {
            requirements[currentDisplayItemIndex].text = "Max Price";
            values[currentDisplayItemIndex].text = contractToDisplay.maxPrice.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesMinPrice)
        {
            requirements[currentDisplayItemIndex].text = "Min Price";
            values[currentDisplayItemIndex].text = contractToDisplay.minPrice.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesUndesirableEffect)
        {
            requirements[currentDisplayItemIndex].text = "Max " + contractToDisplay.undesirableEffect.ToString();
            values[currentDisplayItemIndex].text = contractToDisplay.undesirableEffectMax.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesDesirableEffect)
        {
            requirements[currentDisplayItemIndex].text = "Min " + contractToDisplay.desirableEffect.ToString();
            values[currentDisplayItemIndex].text = contractToDisplay.desirableEffectMin.ToString();
            currentDisplayItemIndex++;
        }

    }


}
