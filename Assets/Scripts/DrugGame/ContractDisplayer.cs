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
    private TextMeshProUGUI spacingDescriptionCopy;
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

    public void DisplayContract()
    {
        // Erase the old data
        companyName.text = "";
        description.text = "";
        spacingDescriptionCopy.text = "<alpha =#00>";

        foreach (TextMeshProUGUI tmpro in requirements)
            tmpro.text = "";
        foreach (TextMeshProUGUI tmpro in values)
            tmpro.text = "";


        Contract contractToDisplay = GameManager.instance.GetCurrentContract();

        companyName.text = contractToDisplay.companyName;
        description.text = contractToDisplay.description;
        spacingDescriptionCopy.text = "<alpha=#00>" + contractToDisplay.description;

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

    public bool EvaluateContract()
    {
        Contract currentContract = GameManager.instance.GetCurrentContract();
        if (currentContract.usesMaxVolatility)
        {
            int vol = DrugManager.instance.GetVol();
            if (vol > currentContract.volatilityMax)
                return false;
        }

        float cost = DrugManager.instance.GetCost();

        if (currentContract.usesMaxPrice)
        {
            if (cost > currentContract.maxPrice)
                return false;
        }

        if (currentContract.usesMinPrice)
        {
            if (cost < currentContract.minPrice)
                return false;
        }

        if (currentContract.usesUndesirableEffect)
        {
            if (DrugManager.instance.undesiredChems > currentContract.undesirableEffectMax)
                return false;
        }

        if (currentContract.usesDesirableEffect)
        {
            if (DrugManager.instance.desiredChems < currentContract.desirableEffectMin)
                return false;
        }

        return true;
    }


}
