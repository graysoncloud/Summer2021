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

        //Printer.instance.solutionPrinted = true;
        //Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;
        //return;

        // Evaluate solution
        if (ContractDisplayer.instance.EvaluateContract())
        {
            Printer.instance.solutionPrinted = true;
            Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;
            OfficeSceneManager.instance.solutionFinished = true;

            // Display a "solution printed", prevent further editing
        }
        else
            Debug.Log("Contract conditions not achived");
    }


}
