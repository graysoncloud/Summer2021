using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneManager : MonoBehaviour
{
    public static OfficeSceneManager instance = null;

    public GameObject background;

    public int currentContractIndex;
    public int contractsSolved;
    private bool holdingNewContract;
    [SerializeField]
    private ContractDisplayer contractDisplayer;

    public bool openedComputerToday;

    public bool solutionFinished;

    public ContractFolder contractPrefab;
    public SolutionPaper solutionPaperPrefab;

    public ContractFolder contractInHand;
    public SolutionPaper solutionInHand;

    // Used to properly return contract / solution paper where it should go
    private GameObject lastLocation;

    public SceneChange officeToDrugGameTransition;
    public SceneChange officeToRecapTransition;
    public SceneChange officeToOSTransition;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Singleton stuff for contractDisplayer
    }

    private void Start()
    {
        currentContractIndex = 0;
        holdingNewContract = false;
        solutionFinished = false;
        openedComputerToday = false;
        Printer.instance.printerPaper.SetActive(false);
    }

    private void Update()
    {
        //Debug.Log(ContractDisplayer.instance == null);

        if (Input.GetMouseButtonDown(0) && !GameManager.instance.optionsMenuActive && !GameManager.instance.sequenceActive)
        {
            Vector3 worldPointOfClick = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 pointOfClick = new Vector3(worldPointOfClick.x, worldPointOfClick.y, 0);

            // Pick Up Contract from Stack
            if (ContractStack.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfClick) && ActiveContractArea.instance.currentContract == null && contractInHand == null && solutionInHand == null)
            {
                if (currentContractIndex < GameManager.instance.currentDay.contracts.Count)
                {
                    Vector3 mouseLocation = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                    holdingNewContract = true;
                    contractInHand = Instantiate(contractPrefab, background.transform);
                    contractInHand.transform.position = new Vector3(mouseLocation.x, mouseLocation.y, 0);

                    contractInHand.pickedUp = true;
                    lastLocation = ContractStack.instance.gameObject;
                }
            }

            // Pick up solution from printer
            else if (Printer.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfClick) && contractInHand == null && solutionInHand == null && Printer.instance.solutionPrinted)
            {
                Vector3 mouseLocation = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                solutionInHand = Instantiate(solutionPaperPrefab, background.transform);
                solutionInHand.transform.position = new Vector3(mouseLocation.x, mouseLocation.y, 0);

                solutionInHand.pickedUp = true;
                Printer.instance.solutionPrinted = false;
                Printer.instance.printerPaper.SetActive(false);

                //lastLocation = printer.instance;
            }

            // Pick up contract from contract area
            else if (ActiveContractArea.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfClick) && ActiveContractArea.instance.currentContract != null && contractInHand == null && solutionInHand == null)
            {
                contractInHand = ActiveContractArea.instance.currentContract;
                ActiveContractArea.instance.currentContract = null;
                contractInHand.pickedUp = true;
                lastLocation = ActiveContractArea.instance.gameObject;
            }

            // Start Drug Game if computer clicked
            else if (OfficeComputer.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfClick) && !solutionFinished && ActiveContractArea.instance.currentContract != null)
            {
                if (!openedComputerToday)
                {
                    SceneChangeManager.instance.StartSceneChange(officeToOSTransition);
                    openedComputerToday = true;
                } else
                {
                    SceneChangeManager.instance.StartSceneChange(officeToDrugGameTransition);
                }
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
                {
                    MusicManager.instance.StartFadeOut();
                    SceneChangeManager.instance.StartSceneChange(officeToRecapTransition);
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPointOfRelease = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 pointOfRelease = new Vector3(worldPointOfRelease.x, worldPointOfRelease.y, 0);

            // Drop contract from hand onto empty activeContractArea
            if (ActiveContractArea.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfRelease) && ActiveContractArea.instance.currentContract == null && contractInHand != null)
            {
                ActiveContractArea.instance.currentContract = contractInHand;
                ActiveContractArea.instance.currentContract.pickedUp = false;
                ActiveContractArea.instance.currentContract.transform.position = ActiveContractArea.instance.contractSpot;
                contractInHand = null;
                lastLocation = null;

                if (holdingNewContract)
                {
                    contractDisplayer.DisplayContract();
                    holdingNewContract = false;
                }
            }

            // Drop solution onto Contract (in ActiveContractArea)
            else if (ActiveContractArea.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfRelease) && ActiveContractArea.instance.currentContract != null && !ActiveContractArea.instance.currentContract.solved && solutionInHand != null)
            {
                // Do something to visually identify being finished
                //ActiveContractArea.instance.currentContract
                ActiveContractArea.instance.currentContract.solved = true;
                Destroy(solutionInHand.gameObject);
                lastLocation = null;
            }

            // Drop completed contract into the filing cabinet
            else if (FilingCabinet.instance.GetComponent<PolygonCollider2D>().bounds.Contains(pointOfRelease) && contractInHand != null && contractInHand.solved)
            {
                EvaluateSolution();
                lastLocation = null;

                contractsSolved++; 
                currentContractIndex++;

                // Trigger dialogue events
                foreach (Day.Sequence sequence in GameManager.instance.currentDay.sequences)
                {
                    if (sequence.trigger.ToString() == "solvedContract1" && contractsSolved == 1)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                        MusicManager.instance.StartFadeOut();
                    }
                    else if (sequence.trigger.ToString() == "solvedContract2" && contractsSolved == 2)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                    }
                    else if (sequence.trigger.ToString() == "solvedContract3" && contractsSolved == 3)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                    }
                    else if (sequence.trigger.ToString() == "solvedContract4" && contractsSolved == 4)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                    }
                    else if (sequence.trigger.ToString() == "solvedContract5" && contractsSolved == 5)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                    }
                    else if (sequence.trigger.ToString() == "solvedContract6" && contractsSolved == 6)
                    {
                        GameManager.instance.StartSequence(sequence.initialEvent);
                        MusicManager.instance.StartFadeOut();
                    }
                }

                if (contractsSolved >= GameManager.instance.currentDay.contracts.Count) 
                    LeaveOfficeButton.instance.gameObject.SetActive(true);

                solutionFinished = false;

                // Maybe have an if / else condition returning a visual error?

            }

            // Return contract to where it came from
            else if (contractInHand != null && lastLocation == ContractStack.instance.gameObject)
            {
                holdingNewContract = false;
                Destroy(contractInHand.gameObject);
            }
            else if (contractInHand != null && lastLocation == ActiveContractArea.instance.gameObject)
            {
                ActiveContractArea.instance.currentContract = contractInHand;
                contractInHand.pickedUp = false;
                contractInHand.transform.position = ActiveContractArea.instance.contractSpot;
                contractInHand = null;
                holdingNewContract = false;
            }
            // Return solution to printer if it's dropped no where special
            else if (solutionInHand != null)
            {
                Destroy(solutionInHand.gameObject);
                Printer.instance.solutionPrinted = true;
                Printer.instance.printerPaper.SetActive(true);
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

    public void SetUpOfficeScene()
    {
        MusicManager.instance.StartBackgroundPlayer();
    }



}
