using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalRotateButton : MonoBehaviour
{
    public Chemical chemical;
    public GameObject toRotate;
    public float amount;

    public bool mouseOver;

    private void Start()
    {
        // The thought was that non-60 degree rotation might be useful at some point, but for now, it will just return an error
        if (Mathf.Abs(amount) != 60)
            Debug.LogError("Invalid rotation amount: " + amount);

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

    /*private void OnMouseDown()
    {
        chemical.RotateConnections(amount);
    }*/

    public void Rotate()
    {
        chemical.RotateConnections(amount);
    }
}
