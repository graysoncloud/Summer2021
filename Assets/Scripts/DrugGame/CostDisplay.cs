using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostDisplay : MonoBehaviour
{
    public static CostDisplay instance = null;

    [SerializeField]
    private TextMeshProUGUI displayText = null;

    [SerializeField]
    private float totalCost = 0;

    private Dictionary<string, int> chemAmount = new Dictionary<string, int>();

    private BinManager bin;
    private Chemical[] chemArray;
    private int currentDayChemIndex;

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
        currentDayChemIndex = GameManager.instance.currentDayIndex;
        bin = GameObject.FindObjectOfType<BinManager>();
        chemArray = bin.GetChemicals();
        foreach (Chemical chem in chemArray)
        {
            chemAmount.Add(chem.name, 0);
        }
    }

    private void OnEnable()
    {
        if(currentDayChemIndex != GameManager.instance.currentDayIndex)
        {
            bin = GameObject.FindObjectOfType<BinManager>();
            chemArray = bin.GetChemicals();
            chemAmount.Clear();
            foreach (Chemical chem in chemArray)
            {
                chemAmount.Add(chem.name, 0);
            }

            currentDayChemIndex = GameManager.instance.currentDayIndex;
        }
    }

    public void AddCost(Chemical chem, float cost)
    {
        if (!chem.isChild)
        {
            float newCost = cost + chemAmount[chem.name];
            totalCost += newCost;
            displayText.text = totalCost.ToString();
            chemAmount[chem.name] += 1;

            ChemicalBin chemBin = bin.GetBin(chem);
            chemBin.ChangeCost(newCost+1);
            ContractDisplayer.instance.UpdatePrice((int) totalCost);
        }
    }

    public void RemoveCost(Chemical chem, float cost)
    {
        if (!chem.isChild)
        {
            chemAmount[chem.name] -= 1;
            float newCost = cost + chemAmount[chem.name];
            totalCost -= newCost;
            displayText.text = totalCost.ToString();

            ChemicalBin chemBin = bin.GetBin(chem);
            chemBin.ChangeCost(newCost);
            ContractDisplayer.instance.UpdatePrice((int)totalCost);
        }
    }

    public float GetCost()
    {
        return totalCost;
    }
}
