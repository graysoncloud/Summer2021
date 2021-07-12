using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneManager : MonoBehaviour
{
    public static OfficeSceneManager instance = null;

    public GameObject background;

    public int currentContractIndex;
    public int contractsSolved;
    [SerializeField]
    private ContractDisplayer contractDisplayer;

    public ContractFolder contractPrefab;
    public SolutionPaper solutionPaperPrefab;

    public ContractFolder contractInHand;
    public SolutionPaper solutionInHand;

    // Used to properly return contract / solution paper where it should go
    private GameObject lastLocation;

    public SceneChange officeToDrugGameTransition;
    public SceneChange officeToRecapTransition;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Singleton stuff for contractDisplayer
    }

    private void Start()
    {
        currentContractIndex = 0;
    }

    private void Update()
    {
        //Debug.Log(ContractDisplayer.instance == null);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPointOfClick = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 pointOfClick = new Vector3(worldPointOfClick.x, worldPointOfClick.y, 0);

            // Pick Up Contract from Stack
            if (ContractStack.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick) && ActiveContractArea.instance.currentContract == null && contractInHand == null && solutionInHand == null)
            {
                if (currentContractIndex < GameManager.instance.currentDay.contracts.Length)
                {
                    contractInHand = Instantiate(contractPrefab, background.transform);
                    contractInHand.pickedUp = true;
                    lastLocation = ContractStack.instance.gameObject;
                }
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
                SceneChangeManager.instance.StartSceneChange(officeToDrugGameTransition);
            }

            // Leave office if button clicked, also looking out for special transitions
            else if (LeaveOfficeButton.instance.GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick))
            {
                bool sequenceTriggered = false;

                foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
                {
                    if (sequence.trigger.ToString() == "leavingWork" && !sequenceTriggered)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        sequenceTriggered = true;
                    }
                }

                if (!sequenceTriggered)
                    SceneChangeManager.instance.StartSceneChange(officeToRecapTransition);
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

                contractDisplayer.DisplayContract(currentContractIndex);
                currentContractIndex++;
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

                contractsSolved++;

                foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
                {
                    if (sequence.trigger.ToString() == "solvedContract1" && contractsSolved == 1)
                        GameManager.instance.StartSequence(sequence.initialEvent);
                    else if (sequence.trigger.ToString() == "solvedContract2" && contractsSolved == 2)
                        GameManager.instance.StartSequence(sequence.initialEvent);
                    else if (sequence.trigger.ToString() == "solvedContract3" && contractsSolved == 3)
                        GameManager.instance.StartSequence(sequence.initialEvent);
                    else if (sequence.trigger.ToString() == "solvedContract4" && contractsSolved == 4)
                        GameManager.instance.StartSequence(sequence.initialEvent);
                    else if (sequence.trigger.ToString() == "solvedContract5" && contractsSolved == 5)
                        GameManager.instance.StartSequence(sequence.initialEvent);
                    else if (sequence.trigger.ToString() == "solvedContract6" && contractsSolved == 6)
                        GameManager.instance.StartSequence(sequence.initialEvent);

                }

                if (contractsSolved >= GameManager.instance.currentDay.contracts.Length) 
                    LeaveOfficeButton.instance.gameObject.SetActive(true);

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
