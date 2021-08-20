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
        // Just delete the true part
        if (true || ContractDisplayer.instance.EvaluateContract())
        {
            Printer.instance.solutionPrinted = true;
            Printer.instance.printerPaper.SetActive(true);
            OfficeSceneManager.instance.solutionFinished = true;

            RecapSceneManager.instance.GenerateFinishedContract();

            SceneChangeManager.instance.StartSceneChange(DrugManager.instance.drugToOfficeSceneChange);

            // Display a "solution printed", prevent further editing
        }

        else
            Debug.Log("Contract conditions not achived");

        // Generate FinishedContract object for the recap scene to read
    }


}
