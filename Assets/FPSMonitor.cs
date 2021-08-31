using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSMonitor : MonoBehaviour
{

    public TextMeshProUGUI text;

    private void Update()
    {
        text.text = (1f / Time.deltaTime).ToString();
    }

}
