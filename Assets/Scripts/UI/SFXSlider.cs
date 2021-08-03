using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SFXSlider : MonoBehaviour
{
    // Generic slider class for any thing that changes music volume
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Start()
    {
        valueText.text = Mathf.Round(slider.value * 100).ToString() + "%";

        //Adds a listener to the main slider and invokes a method when the value changes.
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        valueText.text = Mathf.Round(slider.value * 100).ToString() + "%";
        PlayerPrefs.SetFloat("SFXVolume", slider.value);
    }


}
