using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecapSceneManager : MonoBehaviour
{
    public static RecapSceneManager instance = null;

    public List<FinishedContract> finishedContracts;

    public SceneChange recapToMR;

    public GameObject drugIDParent;
    public GameObject companyNameParent;
    public GameObject descriptionParent;
    public GameObject volatilityParent;
    public GameObject priceParent;
    public GameObject gradeParent;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeDecreasingText;
    public TextMeshProUGUI bonusDecreasingText;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        finishedContracts = new List<FinishedContract>();
    }

    public void GenerateFinishedContract()
    {
        FinishedContract toAdd = new FinishedContract();
        Contract currentContract = GameManager.instance.GetCurrentContract();

        // TO DO: generate drug ID from object in DrugScene
        toAdd.drugID = "(ID)";
        toAdd.companyName = currentContract.companyName;

        toAdd.description = currentContract.description;

        if (currentContract.usesMaxVolatility)
        {
            toAdd.usesMaxVolatility = true;
            toAdd.maxVolatility = currentContract.volatilityMax;
            toAdd.playerVolatility = VolatilityBar.instance.GetVol();
        }

        if (currentContract.usesMaxPrice)
        {
            toAdd.usesMaxPrice = true;
            toAdd.maxPrice = currentContract.maxPrice;
            toAdd.playerMaxPriceVal = (int)CostDisplay.instance.GetCost();
        }

        // Initialized at 1 to match change to denominator
        float gradeDivisor = 1;

        if (toAdd.usesMaxVolatility)
            gradeDivisor += currentContract.optimalMaxVolatilityVal;

        if (toAdd.usesMaxPrice)
            gradeDivisor += currentContract.optimalMaxPriceVal;
        else if (toAdd.usesMinPrice)
            gradeDivisor += CostDisplay.instance.GetCost();

        // Initialized at 1 so there's not divide by 0 errors
        float gradeDenominator = 1;

        if (toAdd.usesMaxVolatility)
            gradeDenominator += VolatilityBar.instance.GetVol();

        if (toAdd.usesMaxPrice)
            gradeDenominator += CostDisplay.instance.GetCost();
        else if (toAdd.usesMinPrice)
            gradeDenominator += currentContract.optimalMinPriceVal;

        float grade = gradeDivisor / gradeDenominator;
        switch (grade)
        {
            case float n when n >= 1:
                toAdd.grade = "A+";
                break;
            case float n when n >= .9f:
                toAdd.grade = "A";
                break;
            case float n when n >= .8f:
                toAdd.grade = "B";
                break;
            case float n when n >= .7f:
                toAdd.grade = "C";
                break;
            default:
                toAdd.grade = "D";
                break;
        }

        finishedContracts.Add(toAdd);

    }

    public void DisplayContracts()
    {
        TextMeshProUGUI[] drugIDs = drugIDParent.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] companyNames = companyNameParent.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] descriptions = descriptionParent.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] volatilities = volatilityParent.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] costs = priceParent.GetComponentsInChildren<TextMeshProUGUI>();
        TextMeshProUGUI[] grades = gradeParent.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < finishedContracts.Count; i++)
        {
            // Turn on fields but set their strings to nothing
            drugIDs[i].gameObject.SetActive(true);
            drugIDs[i].text = "";
            companyNames[i].gameObject.SetActive(true);
            companyNames[i].text = "";
            descriptions[i].gameObject.SetActive(true);
            descriptions[i].text = "";
            volatilities[i].gameObject.SetActive(true);
            volatilities[i].text = "";
            costs[i].gameObject.SetActive(true);
            costs[i].text = "";
            grades[i].gameObject.SetActive(true);
            grades[i].text = "";

            // Assign fields
            drugIDs[i].text = finishedContracts[i].drugID;
            companyNames[i].text = finishedContracts[i].companyName;
            descriptions[i].text = finishedContracts[i].description;

            if (finishedContracts[i].usesMaxVolatility)
                volatilities[i].text = finishedContracts[i].playerVolatility.ToString() + " / " + finishedContracts[i].maxVolatility;
            else
                volatilities[i].text = "NA";

            if (finishedContracts[i].usesMaxPrice)
                costs[i].text = finishedContracts[i].playerMaxPriceVal.ToString() + " / " + finishedContracts[i].maxPrice;
            else if (finishedContracts[i].usesMaxPrice)
                costs[i].text = finishedContracts[i].playerMinPriceVal.ToString() + " / " + finishedContracts[i].minPrice;
            else
                costs[i].text = "NA";

            grades[i].text = finishedContracts[i].grade;
        }


        for (int i = finishedContracts.Count; i < drugIDs.Length; i++)
        {
            drugIDs[i].gameObject.SetActive(false);
            companyNames[i].gameObject.SetActive(false);
            descriptions[i].gameObject.SetActive(false);
            volatilities[i].gameObject.SetActive(false);
            costs[i].gameObject.SetActive(false);
            grades[i].gameObject.SetActive(false);
        }

    }

    public void StartBonusCouroutine()
    {
        StartCoroutine("BonusCouroutine");
    }

    private IEnumerator BonusCouroutine()
    {
        bonusDecreasingText.text = "1000";

        int hours = int.Parse(timeText.text.Substring(0, timeText.text.IndexOf(':')));
        int minutes = int.Parse(timeText.text.Substring(timeText.text.IndexOf(':'), timeText.text.IndexOf(' ')));

        string qualifier = timeText.text.Substring(timeText.text.IndexOf(' '));

        yield return new WaitForSeconds(1f);
    
        while (qualifier != "AM" || minutes != 0 || hours != 9)
        {

            minutes -= 1;
            if (minutes <= -1)
            {
                hours -= 1;
                if (hours <= 0)
                {
                    hours = 12;
                    qualifier = "AM";
                }
            }

            timeDecreasingText.text = hours + ":" + minutes + " " + qualifier;
            bonusDecreasingText.text = (int.Parse(bonusDecreasingText.text) - 1).ToString();

            yield return new WaitForEndOfFrame(); 
        }

    }


}
