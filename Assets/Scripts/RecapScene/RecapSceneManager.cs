using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecapSceneManager : MonoBehaviour
{
    public static RecapSceneManager instance = null;

    public List<FinishedContract> finishedContracts;

    public SceneChange recapToMR;

    public GameObject contractNumbers;
    public GameObject companyNames;
    public GameObject volatilities;
    public GameObject prices;
    public GameObject undesiredEffects;
    public GameObject desiredEffects;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        finishedContracts = new List<FinishedContract>();
    }

    public void GenerateFinishedContract()
    {
        FinishedContract toAdd = new FinishedContract();
        Contract currentContract = GameManager.instance.GetCurrentContract();

        toAdd.companyName = currentContract.companyName;
        
        if (currentContract.usesMaxVolatility)
        {
            toAdd.usesMaxVolatility = true;
            toAdd.maxVolatility = currentContract.volatilityMax;
            toAdd.playerVolatility = VolatilityBar.instance.GetVol();
            toAdd.optimalVolatility = currentContract.optimalMaxVolatilityVal;
        }

        if (currentContract.usesMaxPrice)
        {
            toAdd.usesMaxPrice = true;
            toAdd.maxPrice = currentContract.maxPrice;
            toAdd.playerMaxPriceVal = (int)CostDisplay.instance.GetCost();
            toAdd.optimalMaxPriceVal = currentContract.optimalMaxPriceVal;
        }

        if (currentContract.usesMinPrice)
        {
            toAdd.usesMinPrice = true;
            toAdd.minPrice = currentContract.minPrice;
            toAdd.playerMinPriceVal = (int)CostDisplay.instance.GetCost();
            toAdd.optimalMinPriceVal = currentContract.optimalMinPriceVal;
        }

        if (currentContract.usesDesirableEffect)
        {
            toAdd.usesDesirableEffect = true;
            toAdd.desirableEffect = currentContract.desirableEffect;
            toAdd.desirableEffectMin = currentContract.desirableEffectMin;
            // Needs to be updated
            toAdd.playerDesirableEffectAmount = 10;
            toAdd.optimalDesirableEffectAmount = currentContract.optimalDesirableEffectAmount;
        }

        if (currentContract.usesUndesirableEffect)
        {
            toAdd.usesUndesirableEffect = true;
            toAdd.undesirableEffect = currentContract.undesirableEffect;
            toAdd.undesirableEffectMax = currentContract.undesirableEffectMax;
            // Needs to be updated
            toAdd.playerUndesirableEffectAmount = 10;
            toAdd.optimalUndesirableEffectAmount = currentContract.optimalUndesirableEffectAmount;
        }

        finishedContracts.Add(toAdd);

    }

    public void DisplayContracts()
    {
        TextMeshProUGUI[] contractNums = contractNumbers.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] compNames = companyNames.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] volValues = volatilities.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] costs = prices.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] badEffects = undesiredEffects.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] goodEffects = desiredEffects.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < finishedContracts.Count; i++)
        {
            // Set up fields
            contractNums[i].gameObject.SetActive(true);
            contractNums[i].text = "";
            compNames[i].gameObject.SetActive(true);
            compNames[i].text = "";
            volValues[i].gameObject.SetActive(true);
            volValues[i].text = "";
            costs[i].gameObject.SetActive(true);
            costs[i].text = "";
            badEffects[i].gameObject.SetActive(true);
            badEffects[i].text = "";
            goodEffects[i].gameObject.SetActive(true);
            goodEffects[i].text = "";


            contractNums[i].text = "Contract " + i + ":";
            compNames[i].text = finishedContracts[i].companyName;

            if (finishedContracts[i].usesMaxVolatility)
                volValues[i].text = finishedContracts[i].playerVolatility.ToString() + "(" + finishedContracts[i].optimalVolatility.ToString() + ")/" + finishedContracts[i].maxVolatility;

            if (finishedContracts[i].usesMaxPrice)
                costs[i].text = finishedContracts[i].playerMaxPriceVal.ToString() + "(" + finishedContracts[i].optimalMaxPriceVal.ToString() + ")/" + finishedContracts[i].maxPrice;
            
            if (finishedContracts[i].usesUndesirableEffect)
                badEffects[i].text = finishedContracts[i].playerUndesirableEffectAmount.ToString() + "(" + finishedContracts[i].optimalUndesirableEffectAmount.ToString() + ")/" + finishedContracts[i].undesirableEffectMax;
            
            if (finishedContracts[i].usesDesirableEffect)
                goodEffects[i].text = finishedContracts[i].playerDesirableEffectAmount.ToString() + "(" + finishedContracts[i].optimalDesirableEffectAmount.ToString() + ")/" + finishedContracts[i].desirableEffectMin;
        }


        for (int i = finishedContracts.Count; i < contractNums.Length; i++)
        {
            contractNums[i].gameObject.SetActive(false);
            compNames[i].gameObject.SetActive(false);
            volValues[i].gameObject.SetActive(false);
            costs[i].gameObject.SetActive(false);
            badEffects[i].gameObject.SetActive(false);
            goodEffects[i].gameObject.SetActive(false);
        }




    }


}
