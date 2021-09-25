using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButton : MonoBehaviour
{

    private void OnMouseUp()
    {
        if (Printer.instance.solutionPrinted == true)
        {
            // produce visual error
            return;
        }

        //Printer.instance.solutionPrinted = true;
        //Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;
        //return;

        if (GameManager.instance.optionsMenuActive)
        {
            return;
        }

        // Evaluate solution
        if (true || ContractDisplayer.instance.EvaluateContract())
        {

            DrugManager.instance.EndTutorials();
            
            Printer.instance.solutionPrinted = true;
            Printer.instance.printerPaper.SetActive(true);
            OfficeSceneManager.instance.solutionFinished = true;

            RecapSceneManager.instance.GenerateFinishedContract();

            SceneChangeManager.instance.StartSceneChange(DrugManager.instance.drugToOfficeSceneChange);

            DrugManager.instance.ClearChems();

            if (GameManager.instance.currentDayIndex == 0)
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.ContractTutorial3);

            SFXPlayer.instance.PlaySoundEffect(5);
            // Display a "solution printed", prevent further editing

            Contract currentContract = GameManager.instance.GetCurrentContract();
            bool cleared = ContractDisplayer.instance.EvaluateOptional();
            if (cleared == true)
            {
                if (currentContract.isSpecialContract1)
                {

                    PlayerPrefs.SetInt("SpecialContract1", 1);
                }
                if (currentContract.isSpecialContract2)
                {

                    PlayerPrefs.SetInt("SpecialContract2", 1);
                }

                PlayerPrefs.SetInt("OptionalCompleted", PlayerPrefs.GetInt("OptionalCompleted", 0) + 1);
            }
            else
            {
                if (currentContract.isSpecialContract1)
                {
                    PlayerPrefs.SetInt("SpecialContract1", 0);
                }
                if (currentContract.isSpecialContract2)
                {
                    PlayerPrefs.SetInt("SpecialContract2", 0);
                }
            }
        }
        else
        {
            SFXPlayer.instance.PlaySoundEffect(6);
        }

        // Generate FinishedContract object for the recap scene to read
    }


}
