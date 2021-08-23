using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractDisplayer : MonoBehaviour
{
    public static ContractDisplayer instance = null;

    private Contract contractToDisplay = null;
    private int volIndex = -1, maxPriceIndex = -1, minPriceIndex = -1, badEffectIndex = -1, effectIndex = -1;

    public TextMeshProUGUI companyName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI[] requirements;
    public TextMeshProUGUI[] values;

    public Color baseColor;
    public Color successColor;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void DisplayContract()
    {
        // Erase the old data
        companyName.text = "";
        description.text = "";

        foreach (TextMeshProUGUI tmpro in requirements)
            tmpro.text = "";
        foreach (TextMeshProUGUI tmpro in values)
            tmpro.text = "";


        contractToDisplay = GameManager.instance.GetCurrentContract();

        companyName.text = contractToDisplay.companyName;
        description.text = contractToDisplay.description;

        int currentDisplayItemIndex = 0;

        if (contractToDisplay.usesMaxVolatility)
        {
            volIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Max Volatility";
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.volatilityMax.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesMaxPrice)
        {
            maxPriceIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Max Price";
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.maxPrice.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesMinPrice)
        {
            minPriceIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Min Price";
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.minPrice.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesUndesirableEffect)
        {
            badEffectIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Max " + contractToDisplay.undesirableEffect.ToString();
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.undesirableEffectMax.ToString();
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesDesirableEffect)
        {
            effectIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Min " + contractToDisplay.desirableEffect.ToString();
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.desirableEffectMin.ToString();
            currentDisplayItemIndex++;
        }

    }

    public void UpdateVol(int vol)
    {
        if (contractToDisplay.usesMaxVolatility)
        {
            values[volIndex].text = vol + "/" + contractToDisplay.volatilityMax.ToString();
            if (EvalVol())
            {
                values[volIndex].color = successColor;
                requirements[volIndex].color = successColor;
            }
            else
            {
                values[volIndex].color = baseColor;
                requirements[volIndex].color = baseColor;
            }
        }
    }

    public void UpdatePrice(int price)
    {
        if (contractToDisplay.usesMaxPrice)
        {
            values[maxPriceIndex].text = price + "/" + contractToDisplay.maxPrice.ToString();
            if (EvalMaxPrice())
            {
                values[maxPriceIndex].color = successColor;
                requirements[maxPriceIndex].color = successColor;
            }
            else
            {
                values[maxPriceIndex].color = baseColor;
                requirements[maxPriceIndex].color = baseColor;
            }
        }
        if (contractToDisplay.usesMinPrice)
        {
            values[minPriceIndex].text = price + "/" + contractToDisplay.minPrice.ToString();
            if (EvalMinPrice())
            {
                values[minPriceIndex].color = successColor;
                requirements[minPriceIndex].color = successColor;
            }
            else
            {
                values[minPriceIndex].color = baseColor;
                requirements[minPriceIndex].color = baseColor;
            }
        }
    }

    public void UpdateEffect(int amount)
    {
        if (contractToDisplay.usesDesirableEffect)
        {
            values[effectIndex].text = amount + "/" + contractToDisplay.desirableEffectMin.ToString();
            if (EvalEffect())
            {
                values[effectIndex].color = successColor;
                requirements[effectIndex].color = successColor;
            }
            else
            {
                values[effectIndex].color = baseColor;
                requirements[effectIndex].color = baseColor;
            }
        }
    }

    public void UpdateBadEffect(int amount)
    {
        if (contractToDisplay.usesUndesirableEffect)
        {
            values[badEffectIndex].text = amount + "/" + contractToDisplay.undesirableEffectMax.ToString();
            if (EvalBadEffect())
            {
                values[badEffectIndex].color = successColor;
                requirements[badEffectIndex].color = successColor;
            }
            else
            {
                values[badEffectIndex].color = baseColor;
                requirements[badEffectIndex].color = baseColor;
            }
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
            Debug.Log("Vol good");
        }

        float cost = DrugManager.instance.GetCost();

        if (currentContract.usesMaxPrice)
        {
            if (cost > currentContract.maxPrice)
                return false;
            Debug.Log("Max Price good");
        }

        if (currentContract.usesMinPrice)
        {
            if (cost < currentContract.minPrice)
                return false;
            Debug.Log("Min Price good");
        }

        if (currentContract.usesUndesirableEffect)
        {
            if (DrugManager.instance.undesiredChems > currentContract.undesirableEffectMax)
                return false;
            Debug.Log("Undes good");
        }

        if (currentContract.usesDesirableEffect)
        {
            if (DrugManager.instance.desiredChems < currentContract.desirableEffectMin)
                return false;
            Debug.Log("Des good");
        }

        return true;
    }

    public bool EvalVol()
    {
        int vol = DrugManager.instance.GetVol();
        if (vol > contractToDisplay.volatilityMax)
            return false;
        return true;
    }
    public bool EvalMaxPrice()
    {
        float cost = DrugManager.instance.GetCost();
        if (cost > contractToDisplay.maxPrice)
            return false;
        return true;
    }
    public bool EvalMinPrice()
    {
        float cost = DrugManager.instance.GetCost();
        if (cost < contractToDisplay.minPrice)
            return false;
        return true;
    }
    public bool EvalEffect()
    {
        if (DrugManager.instance.desiredChems < contractToDisplay.desirableEffectMin)
            return false;
        return true;
    }
    public bool EvalBadEffect()
    {
        if (DrugManager.instance.undesiredChems > contractToDisplay.undesirableEffectMax)
            return false;
        return true;
    }
}
