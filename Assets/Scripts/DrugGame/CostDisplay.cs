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

    private Dictionary<string, int> chemAmount;

    private BinManager bin;

    private void Awake()
    {
        bin = GameObject.FindObjectOfType<BinManager>();
    }

    public void UpdateCost(float amount)
    {
        totalCost += amount;
        displayText.text = totalCost.ToString();
    }

}
