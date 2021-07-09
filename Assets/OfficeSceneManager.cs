using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneManager : MonoBehaviour
{
    public static OfficeSceneManager instance = null;

    public GameObject background;

    public ContractFolder contractPrefab;
    public SolutionPaper solutionPaperPrefab;

    public ContractFolder contractInHand;
    public SolutionPaper solutionInHand;

    // Used to properly return contract / solution paper where it should go
    private GameObject lastLocation;

    public SceneChange computerToDrugGameTransition;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPointOfClick = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 pointOfClick = new Vector3(worldPointOfClick.x, worldPointOfClick.y, 0);

            // Pick Up Contract from Stack
            if (ContractStack.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick) && ActiveContractArea.instance.currentContract == null && contractInHand == null && solutionInHand == null)
            {
                contractInHand = Instantiate(contractPrefab, background.transform);
                contractInHand.pickedUp = true;
                lastLocation = ContractStack.instance.gameObject;
            }

            // Pick up solution from printer
            else if (Printer.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick) && contractInHand == null && solutionInHand == null && Printer.instance.solutionPrinted)
            {
                solutionInHand = Instantiate(solutionPaperPrefab, background.transform);
                solutionInHand.pickedUp = true;
                Printer.instance.solutionPrinted = false;
                Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.noSolutionSprite;

                //lastLocation = printer.instance;
            }

            // Pick up contract from contract area
            else if (ActiveContractArea.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick) && ActiveContractArea.instance.currentContract != null && contractInHand == null && solutionInHand == null)
            {
                contractInHand = ActiveContractArea.instance.currentContract;
                ActiveContractArea.instance.currentContract = null;
                contractInHand.pickedUp = true;
                lastLocation = ActiveContractArea.instance.gameObject;
            }

            // Start Drug Game if computer clicked
            else if (OfficeComputer.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick))
            {
                SceneChangeManager.instance.StartSceneChange(computerToDrugGameTransition);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPointOfRelease = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 pointOfRelease = new Vector3(worldPointOfRelease.x, worldPointOfRelease.y, 0);

            // Drop contract from hand onto empty activeContractArea
            if (ActiveContractArea.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfRelease) && ActiveContractArea.instance.currentContract == null && contractInHand != null)
            {
                ActiveContractArea.instance.currentContract = contractInHand;
                ActiveContractArea.instance.currentContract.pickedUp = false;
                ActiveContractArea.instance.currentContract.transform.position = ActiveContractArea.instance.contractSpot;
                contractInHand = null;
                lastLocation = null;

                // Notify drug game that the next contract is ready to go
            }

            // Drop solution onto Contract (in ActiveContractArea)
            else if (ActiveContractArea.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfRelease) && ActiveContractArea.instance.currentContract != null && !ActiveContractArea.instance.currentContract.solved && solutionInHand != null)
            {
                ActiveContractArea.instance.currentContract.GetComponent<SpriteRenderer>().sprite = ActiveContractArea.instance.currentContract.solvedContractSprite;
                ActiveContractArea.instance.currentContract.solved = true;
                Destroy(solutionInHand.gameObject);
                lastLocation = null;
            }

            // Drop completed contract into the filing cabinet
            else if (FilingCabinet.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfRelease) && contractInHand != null && contractInHand.solved)
            {
                EvaluateSolution();
                lastLocation = null;

                // Maybe have an if / else condition returning a visual error?
            }

            // Return contract to where it came from
            else if (contractInHand != null && lastLocation == ContractStack.instance.gameObject)
            {
                Destroy(contractInHand.gameObject);
            }
            else if (contractInHand != null && lastLocation == ActiveContractArea.instance.gameObject)
            {
                ActiveContractArea.instance.currentContract = contractInHand;
                contractInHand.pickedUp = false;
                contractInHand.transform.position = ActiveContractArea.instance.contractSpot;
                contractInHand = null;
            }
            // Return solution to printer if it's dropped no where special
            else if (solutionInHand != null)
            {
                Destroy(solutionInHand.gameObject);
                Printer.instance.solutionPrinted = true;
                Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;
            }
        }

        // Should be deleted- only a test mechanism
        //if (Input.GetKeyDown("space"))
        //{
        //    Printer.instance.solutionPrinted = true;
        //    Printer.instance.GetComponent<SpriteRenderer>().sprite = Printer.instance.printedSolutionSprite;
        //}

    }

    private void EvaluateSolution()
    {
        Destroy(contractInHand.gameObject);
        // Do something to determine how the player did
    }



}
