using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayText = null;

    [SerializeField]
    private float totalCost = 0;

    private Dictionary<string, int> chemAmount = new Dictionary<string, int>();

    private BinManager bin;
    private Chemical[] chemArray;

    private void Start()
    {
        bin = GameObject.FindObjectOfType<BinManager>();
        chemArray = bin.GetChemicals();
        foreach (Chemical chem in chemArray)
        {
            chemAmount.Add(chem.name, 0);
        }
    }

    public void AddCost(Chemical chem, float cost)
    {
        if (!chem.isChild)
        {
            totalCost += cost + (chemAmount[chem.name] * chemAmount[chem.name]);
            displayText.text = totalCost.ToString();
            chemAmount[chem.name] += 1;
        }
    }

    public void RemoveCost(Chemical chem, float cost)
    {
        if (!chem.isChild)
        {
            chemAmount[chem.name] -= 1;
            totalCost -= cost + (chemAmount[chem.name] * chemAmount[chem.name]);
            displayText.text = totalCost.ToString();
        }
    }

    public float GetCost()
    {
        return totalCost;
    }
}
