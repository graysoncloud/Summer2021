using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolatilityBar : MonoBehaviour
{
    public static VolatilityBar instance = null;

    [SerializeField]
    private float volatility;
    private int volMax;

    [SerializeField]
    private Text volatilityText = null;
    /*[SerializeField]
    private SpriteRenderer volatilityBar = null, volatiltiyBG = null;*/
    [SerializeField]
    CircleSlider volBar = null;
    [SerializeField]
    private float unstableVol = 10, negativeVol = 1.5f, positiveVol = -1;
    private int unstableCount = 0;

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
        Refresh();
    }

    // Takes in the 12 statuses that were possibly altered from a rotation, placement or pickup, and adjusts the danger level accordingly
    public void UpdateDanger(string[] oldStatuses, string[] newStatuses)
    {
        for (int i = 0; i < oldStatuses.Length; i++)
        {
            // expand if else statements to increase separate variables to display different bars, i.e. unstable amount vs. negative amount
            if (newStatuses[i] == "Unstable")
            {
                volatility += unstableVol;
                unstableCount++;
            }
            else if (newStatuses[i] == "Negative")
                volatility += negativeVol;
            else if (newStatuses[i] == "Positive")
                volatility += positiveVol;


            // This essentially reverses the above affect if nothing changed
            if (oldStatuses[i] == "Unstable")
            {
                volatility -= unstableVol;
                unstableCount--;
            }
            else if (oldStatuses[i] == "Negative")
                volatility -= negativeVol;
            else if (oldStatuses[i] == "Positive")
                volatility -= positiveVol;
        }

        volatility = (int)volatility;

        volatilityText.text = Mathf.Clamp(volatility, 0, 999).ToString() + " / " + volMax.ToString();
        ContractDisplayer.instance.UpdateVol((int)volatility);
        /*if (volatility < 0)
        {
            volatilityText.text = "0";
            volatilityBar.transform.localScale = new Vector3(0, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        } else if (volatility * 4 < volatiltiyBG.transform.localScale.x * volatiltiyBG.size.x)
        {
            volatilityBar.transform.localScale = new Vector3(volatility * 4, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        } else
        {
            volatilityBar.transform.localScale = new Vector3(volatiltiyBG.transform.localScale.x * volatiltiyBG.size.x, volatilityBar.transform.localScale.y, volatilityBar.transform.localScale.z);
        }*/

        float volAmount = (float)volatility / (float)volMax;
        Mathf.Clamp(volAmount, 0, 1);
        volBar.ChangeProgress(volAmount);
    }

    public void Refresh()
    {
        volatility = 0;
        int contractMax = DrugManager.instance.GetVolMax();
        if (contractMax != -1)
        {
            volMax = contractMax;
            volatilityText.text = "0 / " + volMax.ToString();
        }
        else
        {
            //disable if no vol max
            this.gameObject.SetActive(false);
        }
    }

    public int GetVol()
    {
        return (int)volatility;
    }

    public int GetUnstableCount()
    {
        return unstableCount;
    }
}
