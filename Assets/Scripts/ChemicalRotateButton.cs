using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalRotateButton : MonoBehaviour
{
    public GameObject Chemical;
    public float amount;

    public bool mouseOver;

    private void Start()
    {
        mouseOver = false;
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        StartCoroutine("RotateEnum");
    }

    IEnumerator RotateEnum()
    {
        for (float i = 0; i < Mathf.Abs(amount); i += (Mathf.Abs(amount) / 20))
        {
            Chemical.transform.Rotate(0, 0, amount / 20);
            yield return new WaitForSeconds(.002f);
        }
    }
}
