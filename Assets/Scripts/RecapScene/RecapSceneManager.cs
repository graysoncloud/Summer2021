using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

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

    public UIButton nextDayButton;

    public GameObject registerOverlay;
    public GameObject bonusOverlay;

    public TextMeshProUGUI drugGameTimeText;
    public TextMeshProUGUI timeDecreasingText;
    public TextMeshProUGUI bonusDecreasingText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI grossAmountText;

    private string charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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
        
        // Create random ID
        string randomString = "";
       
        // Change the i < 4 bit if you want more chars
        for (int i = 0; i < 4; i++)
            randomString += charList[UnityEngine.Random.Range(0, 25)];

        toAdd.drugID = randomString + "#" + PlayerPrefs.GetInt("DrugID").ToString();
        PlayerPrefs.SetInt("DrugID", PlayerPrefs.GetInt("DrugID") + UnityEngine.Random.Range(1, 5));

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
        float gradeDividend = 1;

        if (toAdd.usesMaxVolatility)
        {
            gradeDividend += currentContract.optimalMaxVolatilityVal;
        }

        if (toAdd.usesMaxPrice)
        {
            gradeDividend += currentContract.optimalMaxPriceVal;
        }
        else if (toAdd.usesMinPrice)
            gradeDividend += CostDisplay.instance.GetCost();

        // Initialized at 1 so there's not divide by 0 errors
        float gradeDivisor = 1;

        if (toAdd.usesMaxVolatility)
            gradeDivisor += VolatilityBar.instance.GetVol();

        if (toAdd.usesMaxPrice)
            gradeDivisor += CostDisplay.instance.GetCost();
        else if (toAdd.usesMinPrice)
            gradeDivisor += currentContract.optimalMinPriceVal;

        Debug.Log(gradeDividend + ", / " + gradeDivisor);

        float grade = gradeDividend / gradeDivisor;
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
        bonusOverlay.gameObject.SetActive(false);
        registerOverlay.gameObject.SetActive(true);

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

    public void RevealBonusScene()
    {
        nextDayButton.GetComponent<EventTrigger>().enabled = false;

        string minutesAsString = DrugManager.instance.minutes.ToString();
        if (DrugManager.instance.minutes < 10)
            minutesAsString = "0" + DrugManager.instance.minutes;

        timeDecreasingText.text = DrugManager.instance.hours + ":" + minutesAsString + " " + DrugManager.instance.qualifier;

        totalText.gameObject.SetActive(false);
        grossAmountText.gameObject.SetActive(false);
        registerOverlay.gameObject.SetActive(false);
        bonusOverlay.gameObject.SetActive(true);

        StartCoroutine("BonusCouroutine");
    }

    private IEnumerator BonusCouroutine()
    {
        // Maybe increase salary over time?
        bonusDecreasingText.text = "$1000";

        int hours = DrugManager.instance.hours;
        int minutes = DrugManager.instance.minutes;

        string qualifier = DrugManager.instance.qualifier;
        yield return new WaitForSeconds(1f);
    
        while (qualifier != "AM" || minutes != 0 || hours != 9)
        {

            minutes -= 1;
            if (minutes < 0)
            {
                hours -= 1;
                minutes = 59;
                if (hours <= 0)
                {
                    hours = 12;
                    qualifier = "AM";
                }
            }

            string minutesAsString = minutes.ToString();
            if (minutes < 10)
                minutesAsString = "0" + minutes.ToString();

            timeDecreasingText.text = hours + ":" + minutesAsString + " " + qualifier;
            bonusDecreasingText.text = "$" + (int.Parse(bonusDecreasingText.text.Substring(1)) - 1).ToString();

            yield return new WaitForEndOfFrame(); 
        }

        yield return new WaitForSeconds(1.2f);

        totalText.gameObject.SetActive(true);

        PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney") + int.Parse(bonusDecreasingText.text.Substring(1)));
        grossAmountText.text = "$" + PlayerPrefs.GetInt("TotalMoney").ToString();

        yield return new WaitForSeconds(.8f);

        grossAmountText.gameObject.SetActive(true);

        yield return new WaitForSeconds(.5f);

        nextDayButton.GetComponent<EventTrigger>().enabled = true;
    }

    public void AdvanceToNextDay()
    {
        foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
        {
            if (sequence.trigger.ToString() == "dream")
            {
                GameManager.instance.StartSequence(sequence.initialEvent);
                return;
            }
        }
        SceneChangeManager.instance.StartSceneChange(RecapSceneManager.instance.recapToMR);
    }


}
