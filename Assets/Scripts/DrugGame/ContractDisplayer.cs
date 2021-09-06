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

    public Color baseColor, successColor, optionColor, optionSuccess;

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
            values[currentDisplayItemIndex].color = successColor;
            requirements[currentDisplayItemIndex].color = successColor;
            currentDisplayItemIndex++;
        }

        if (contractToDisplay.usesMaxPrice)
        {
            maxPriceIndex = currentDisplayItemIndex;
            requirements[currentDisplayItemIndex].text = "Max Price";
            values[currentDisplayItemIndex].text = "0/" + contractToDisplay.maxPrice.ToString();
            values[currentDisplayItemIndex].color = successColor;
            requirements[currentDisplayItemIndex].color = successColor;
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

        if (contractToDisplay.usesOptional)
        {
            if (currentDisplayItemIndex == 5)
            {
                Debug.LogWarning("Nathan is lazy so he won't code for a full contract with an optional condition, go yell at him");
                return;
            }
            if (contractToDisplay.usesOptionalDesireable)
            {
                requirements[4].text = "Min " + contractToDisplay.optionalEffect.ToString(); //Change 4 to length - 1 in the future aka never
                values[4].text = "0/" + contractToDisplay.optionalDesirableMin.ToString();
            } 
            else if (contractToDisplay.usesOptionalUndesirable)
            {
                requirements[4].text = "Max " + contractToDisplay.optionalEffect.ToString();
                values[4].text = "0/" + contractToDisplay.optionalUndesirableMax.ToString();
            } 
            else if (contractToDisplay.usesOptionalMaxPrice)
            {
                requirements[4].text = "Max Price";
                values[4].text = "0/" + contractToDisplay.optionalPriceMax.ToString();
                values[4].color = optionSuccess;
                requirements[4].color = optionSuccess;
            }
            else if (contractToDisplay.usesOptionalMinPrice)
            {
                requirements[4].text = "Min Price";
                values[4].text = "0/" + contractToDisplay.optionalPriceMin.ToString();
            }
            else if (contractToDisplay.usesOptionalVol)
            {
                requirements[4].text = "Max Volatility";
                values[4].text = "0/" + contractToDisplay.optionalVolMax.ToString();
                values[4].color = optionSuccess;
                requirements[4].color = optionSuccess;
            }
            else
            {
                Debug.LogWarning("Optional condition not found, make sure you've checked a condition");
            }
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
        if (contractToDisplay.usesOptionalVol)
        {
            values[4].text = vol + "/" + contractToDisplay.optionalVolMax.ToString();
            if (EvalOptionalVol())
            {
                values[4].color = optionSuccess;
                requirements[4].color = optionSuccess;
            }
            else
            {
                values[4].color = optionColor;
                requirements[4].color = optionColor;
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
        if (contractToDisplay.usesOptionalMaxPrice)
        {
            values[4].text = price + "/" + contractToDisplay.optionalPriceMax.ToString();
            if (EvalOptionalMaxPrice())
            {
                values[4].color = optionSuccess;
                requirements[4].color = optionSuccess;
            }
            else
            {
                values[4].color = optionColor;
                requirements[4].color = optionColor;
            }
        } 
        else if (contractToDisplay.usesOptionalMinPrice)
        {
            values[4].text = price + "/" + contractToDisplay.optionalPriceMin.ToString();
            if (EvalOptionalMinPrice())
            {
                values[4].color = optionSuccess;
                requirements[4].color = optionSuccess;
            }
            else
            {
                values[4].color = optionColor;
                requirements[4].color = optionColor;
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

    public void UpdateOptional(int amount)
    {
        if (contractToDisplay.usesOptionalDesireable || contractToDisplay.usesOptionalUndesirable)
        {
            if (contractToDisplay.usesOptionalUndesirable)
                values[4].text = amount + "/" + contractToDisplay.optionalUndesirableMax.ToString();
            else
                values[4].text = amount + "/" + contractToDisplay.optionalDesirableMin.ToString();
            if (EvalOptionalEffect())
            {
                values[badEffectIndex].color = optionSuccess;
                requirements[badEffectIndex].color = optionSuccess;
            }
            else
            {
                values[badEffectIndex].color = optionColor;
                requirements[badEffectIndex].color = optionColor;
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

        if (currentContract.usesOptional)
        {
            bool cleared = true;
            if (currentContract.usesOptionalDesireable)
            {
                if (DrugManager.instance.optionalChems < currentContract.optionalDesirableMin)
                    cleared = false;
            } else if (currentContract.usesOptionalUndesirable)
            {
                if (DrugManager.instance.optionalChems > currentContract.optionalUndesirableMax)
                    cleared = false;
            } else if (currentContract.usesOptionalMinPrice)
            {
                if (cost > currentContract.optionalPriceMax)
                    cleared = false;
            } else if (currentContract.usesOptionalMaxPrice)
            {
                if (cost < currentContract.optionalPriceMin)
                    cleared = false;
            } else if (currentContract.usesOptionalVol)
            {
                int vol = DrugManager.instance.GetVol();
                if (vol > currentContract.optionalVolMax)
                    cleared = false;
            }
            else
            {
                Debug.LogWarning("Optional Contract with no condition");
            }

            if (cleared == true)
            {
                if (currentContract.isSpecialContract1)
                {
                    PlayerPrefs.SetInt("SpecialContract1", 1);
                }
                if (currentContract.isSpecialContract2)
                {
                    PlayerPrefs.SetInt("SpecialContract2", 1);
                }
                PlayerPrefs.SetInt("OptionalCompleted", PlayerPrefs.GetInt("OptionalCompleted", 0) + 1);
            } else
            {
                if (currentContract.isSpecialContract1)
                {
                    PlayerPrefs.SetInt("SpecialContract1", 0);
                }
                if (currentContract.isSpecialContract2)
                {
                    PlayerPrefs.SetInt("SpecialContract2", 0);
                }
            }
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
    public bool EvalOptionalVol()
    {
        int vol = DrugManager.instance.GetVol();
        if (vol > contractToDisplay.optionalVolMax)
            return false;
        return true;
    }
    public bool EvalOptionalMaxPrice()
    {
        float cost = DrugManager.instance.GetCost();
        if (cost > contractToDisplay.optionalPriceMax)
            return false;
        return true;
    }
    public bool EvalOptionalMinPrice()
    {
        float cost = DrugManager.instance.GetCost();
        if (cost < contractToDisplay.optionalPriceMin)
            return false;
        return true;
    }
    public bool EvalOptionalEffect()
    {
        if (DrugManager.instance.optionalChems < contractToDisplay.optionalDesirableMin)
            return false;
        return true;
    }
    public bool EvalOptionalBadEffect()
    {
        if (DrugManager.instance.optionalChems > contractToDisplay.optionalUndesirableMax)
            return false;
        return true;
    }
}
