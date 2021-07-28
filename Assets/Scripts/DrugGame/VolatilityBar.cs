using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolatilityBar : MonoBehaviour
{
    public static VolatilityBar instance = null;

    [SerializeField]
    private int volatility;

    // Will probably be replaced by bar apparatus
    [SerializeField]
    private TextMeshProUGUI volatilityText = null;
    [SerializeField]
    private SpriteRenderer volatilityBar = null, volatiltiyBG = null;
    [SerializeField]
    private int unstableVol = 5, negativeVol = 2, positiveVol = -1;

    //private int unstableIncrement = 4;
    //private int negativeConnectionIncrement = 1;

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
        volatility = 0;
    }

    // Takes in the 12 statuses that were possibly altered from a rotation, placement or pickup, and adjusts the danger level accordingly
    public void UpdateDanger(string[] oldStatuses, string[] newStatuses)
    {
        for (int i = 0; i < oldStatuses.Length; i++)
        {
            // expand if else statements to increase separate variables to display different bars, i.e. unstable amount vs. negative amount
            if (newStatuses[i] == "Unstable")
                volatility += unstableVol;
            else if (newStatuses[i] == "Negative")
                volatility += negativeVol;
            else if (newStatuses[i] == "Positive")
                volatility += positiveVol;


            // This essentially reverses the above affect if nothing changed
            if (oldStatuses[i] == "Unstable")
                volatility -= unstableVol;
            else if (oldStatuses[i] == "Negative")
                volatility -= negativeVol;
            else if (oldStatuses[i] == "Positive")
                volatility -= positiveVol;
        }

        volatilityText.text = volatility.ToString();
        if (volatility < 0)
        {
            volatilityText.text = "0";
            volatilityBar.transform.localScale = new Vector3(0, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        } else if (volatility * 4 < volatiltiyBG.transform.localScale.x * volatiltiyBG.size.x)
        {
            volatilityBar.transform.localScale = new Vector3(volatility * 4, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        } else
        {
            volatilityBar.transform.localScale = new Vector3(volatiltiyBG.transform.localScale.x * volatiltiyBG.size.x, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        }
        
    }

    public int GetVol()
    {
        return volatility;
    }
}
