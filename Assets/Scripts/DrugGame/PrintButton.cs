using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButton : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (Printer.instance.solutionPrinted == true)
        {
            // produce visual error
            return;
        }

        // Evaluate solution

        Printer.instance.solutionPrinted = true;
        Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;

        // Display a "solution printed", prevent further editing
    }


}
