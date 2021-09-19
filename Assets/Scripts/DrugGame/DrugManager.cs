using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrugManager : MonoBehaviour
{
    public static DrugManager instance = null;

    [SerializeField]
    private CostDisplay costDisplay = null;

    [SerializeField]
    private Chemical childChem = null;

    // Some way to hold the current solution

    public Chemical currentlyHeldChemical;
    private Chemical lastHovered;
    private HexGrid hexGrid;

    public SceneChange drugToOfficeSceneChange;

    private VolatilityBar dangerBar;

    public int desiredChems = 0, undesiredChems = 0, optionalChems = 0;

    [SerializeField]
    private TextMeshProUGUI timeText;
    private int lastTimeStamp;
    private float timeElapsed;

    public int hours;
    public int minutes;
    public string qualifier;

    public delegate void OnClearChems();
    public static event OnClearChems onClearChems;

    public int numtutorialsfinished;
    public bool tutorialsfinished;
    public int numrecapfinished;
    public bool allrecapfinished;
    private Vector2 tutorialTheromideTile = new Vector2(2f, 5f);
    private HexTile[] tutorialAveroTiles;
    private Color tutorialHighlightColor = new Color(0.9f, 0.6f, 0.2f, 1f);

    public GameObject hexGraphics;
    public GameObject centerGraphics1;
    public GameObject centerGraphics2;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {

        numtutorialsfinished = 0;
        tutorialsfinished = false;
        numrecapfinished = 0;
        allrecapfinished = true;

        hexGrid = GameObject.FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
        dangerBar = GameObject.FindObjectOfType<VolatilityBar>();


        lastTimeStamp = 0;
        timeElapsed = 0;

        hours = 9;
        minutes = 0;
        qualifier = "AM";
    }

    private bool TutorialContainsAvero()
    {
        foreach(HexTile tile in tutorialAveroTiles)
        {
            if(tile.storedChemical != null && tile.storedChemical.name == "Avero")
            {
                return true;
            }
        }
        return false;
    }
    void Update()
    {
        if(!tutorialsfinished && GameManager.instance.currentDayIndex == 0){
            if(numtutorialsfinished == 0 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial1);
                numtutorialsfinished++;
            }
            if(numtutorialsfinished == 1 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial2);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 2 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial3);
                numtutorialsfinished++;
                hexGrid.GetHexTile(tutorialTheromideTile).LockColor(tutorialHighlightColor);
            }
            //Looks very ugly, but have to make sure the chemical exists first.
            else if(numtutorialsfinished == 3 && TutorialManager.instance.activeTutorial == null && hexGrid.GetHexTile(tutorialTheromideTile).storedChemical != null && hexGrid.GetHexTile(tutorialTheromideTile).storedChemical.name == "Theromide")
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial4);
                numtutorialsfinished++;
                hexGrid.GetHexTile(tutorialTheromideTile).UnlockColor();
            }
            else if(numtutorialsfinished == 4 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial5);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 5 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial6);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 6 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial7);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 7 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial8);
                numtutorialsfinished++;
                tutorialAveroTiles = hexGrid.GetAdjacent(tutorialTheromideTile);
                foreach(HexTile tile in tutorialAveroTiles)
                {
                    tile.LockColor(tutorialHighlightColor);
                }
            }
            else if(numtutorialsfinished == 8 && TutorialManager.instance.activeTutorial == null && dangerBar.GetVol() == 33 && TutorialContainsAvero())
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial9);
                numtutorialsfinished++;
                foreach(HexTile tile in tutorialAveroTiles)
                {
                    tile.UnlockColor();
                }
            }
            else if(numtutorialsfinished == 9 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial10);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 10 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial11);
                numtutorialsfinished++;
            }
            else if(numtutorialsfinished == 11 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.DrugGameTutorial12);
                numtutorialsfinished++;
                tutorialsfinished = true;
            }
        }

        if(!tutorialsfinished && GameManager.instance.currentDayIndex == 1){
            if(numtutorialsfinished == 0 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day2Tutorial1);
                numtutorialsfinished++;
            }
            if(numtutorialsfinished == 1 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day2Tutorial2);
                numtutorialsfinished++;
            }
            if(numtutorialsfinished == 2 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day2Tutorial3);
                numtutorialsfinished++;
                tutorialsfinished = true;
            }
        }

        if(!tutorialsfinished && GameManager.instance.currentDayIndex == 2){
            if(numtutorialsfinished == 0 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day3Tutorial1);
                numtutorialsfinished++;
            }
            if(numtutorialsfinished == 1 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day3Tutorial2);
                numtutorialsfinished++;
            }
            if(numtutorialsfinished == 2 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateTutorial(TutorialManager.instance.Day3Tutorial3);
                numtutorialsfinished++;
                tutorialsfinished = true;
            }
        }

        if(!allrecapfinished)
        {
            if(numrecapfinished == 0 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateReplayableTutorial(TutorialManager.instance.RecapTutorial1);
                numrecapfinished++;
            }
            else if(numrecapfinished == 1 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateReplayableTutorial(TutorialManager.instance.RecapTutorial2);
                numrecapfinished++;
            }
            if(numrecapfinished == 2 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateReplayableTutorial(TutorialManager.instance.RecapTutorial3);
                numrecapfinished++;
            }
            if(numrecapfinished == 3 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateReplayableTutorial(TutorialManager.instance.RecapTutorial4);
                numrecapfinished++;
            }
            else if(numrecapfinished == 4 && TutorialManager.instance.activeTutorial == null)
            {
                TutorialManager.instance.ActivateReplayableTutorial(TutorialManager.instance.RecapTutorial5);
                numrecapfinished++;
                allrecapfinished = true;
            }

            
        }

        // Multiply by 30 for speedy
        timeElapsed += Time.deltaTime;

        // Every fifteen seconds in the drug game, the player-visible clock will tick up 15 minutes. Stops at 11:00
        if (Mathf.Floor(timeElapsed / 15) > lastTimeStamp && (timeElapsed < 854))
        {
            lastTimeStamp++;
            hours = (int)Mathf.Floor(lastTimeStamp / 4) + 9;

            if (hours == 12)
                qualifier = "PM";

            if (hours >= 13)
                hours -= 12;

            if (timeElapsed > 480)
                PlayerPrefs.SetInt("IsLate", 1);

            minutes = ((lastTimeStamp % 4) * 15);
            string minutesAsString = minutes.ToString();
            if (minutes == 0)
                minutesAsString = "00";

            timeText.text = hours.ToString() + ":" + minutesAsString + " " + qualifier;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f);  

        if (hit.transform != null && !GameManager.instance.optionsMenuActive && !TutorialManager.instance.IsTutorialActive() && currentlyHeldChemical == null) //hover over stuff
        {
            bool chemHovered = false;
            if (hit.transform.gameObject.tag == "Chemical" || hit.transform.gameObject.tag == "Rotate")
            {
                chemHovered = true;
            }
            if (lastHovered != null && chemHovered)
            {
                lastHovered.setActive(false); //for when the last hovered was a different chemical
            }
            if (hit.transform.gameObject.tag == "Chemical")
            {
                lastHovered = hit.transform.GetComponent<Chemical>();
            }
            if (hit.transform.gameObject.tag == "Rotate")
            {
                ChemicalRotateButton button = hit.transform.GetComponent<ChemicalRotateButton>();
                if (button != null)
                {
                    lastHovered = button.transform.GetComponentInParent<Chemical>();
                }
            }
            if (chemHovered)
            {
                lastHovered.setActive(true);
            }
            else
            {
                if (lastHovered != null)
                {
                    lastHovered.setActive(false);
                }
            }

            if (Input.GetButtonDown("Rotate") && chemHovered)
            {
                float rotateDir = Input.GetAxis("Rotate");
                lastHovered.RotateConnections(60 * -rotateDir);
            }
        }

        // rotate in hand
        if (currentlyHeldChemical != null && !GameManager.instance.optionsMenuActive && !TutorialManager.instance.IsTutorialActive())
        {
            if (Input.GetButtonDown("Rotate")) {
                float rotateDir = Input.GetAxis("Rotate");
                currentlyHeldChemical.RotateConnections(60 * -rotateDir);
            }
        }

        if (Input.GetMouseButtonDown(0) && !GameManager.instance.optionsMenuActive && !TutorialManager.instance.IsTutorialActive())
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Rotate")
                {
                    ChemicalRotateButton button = hit.transform.GetComponent<ChemicalRotateButton>();
                    if (button != null)
                    {
                        button.Rotate();
                    }
                }
                
                if (hit.transform.gameObject.tag == "Bin")
                {
                    ChemicalBin bin = hit.transform.GetComponent<ChemicalBin>();
                    if (bin != null)
                    {
                        bin.CreateDrug();
                    }
                }
            }
        }
        

        if (!Input.GetMouseButton(0) && !GameManager.instance.optionsMenuActive && !TutorialManager.instance.IsTutorialActive())
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Grid")
                {
                    HexTile tile = hit.transform.GetComponent<HexTile>();
                    if (tile != null)
                    {
                        tile.DropChem();
                    }
                }
                 
                if (hit.transform.gameObject.tag == "Trash")
                {
                    Garbage trash = hit.transform.GetComponent<Garbage>();
                    if (trash != null)
                    {
                        TrashChem();
                    }
                }

                if (hit.transform.gameObject.tag == "Bin")
                {
                    ChemicalBin bin = hit.transform.GetComponent<ChemicalBin>();
                    if (bin != null)
                    {
                        TrashChem();
                    }
                }
            }
            else
            {
                if (currentlyHeldChemical != null)
                {
                    TrashChem();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearChems();
        }

    }

    public void ClearChems()
    {
        if (onClearChems != null)
        {
            onClearChems();
        }
    }

    public int GetVol()
    {
        return dangerBar.GetVol();
    }

    public float GetCost()
    {
        return costDisplay.GetCost();
    }

    public int GetUnstableCount()
    {
        return dangerBar.GetUnstableCount();
    }

    public EffectType GetDesireable()
    {
        return GameManager.instance.GetCurrentContract().desirableEffect;
    }

    public EffectType GetUndesireable()
    {
        return GameManager.instance.GetCurrentContract().undesirableEffect;
    }

    public EffectType GetOptionalEffect()
    {
        return GameManager.instance.GetCurrentContract().optionalEffect;
    }

    public int GetVolMax()
    {
        if (GameManager.instance.GetCurrentContract().usesMaxVolatility)
        {
            return GameManager.instance.GetCurrentContract().volatilityMax;
        }
        else
            return -1;
        
    }

    public void SetConnection(Chemical chemical, int index, string type)
    {
        chemical.connectionTypes[index] = type;
        chemical.EvaluateConnections();
    }

    public void TrashChem()
    {
        if (currentlyHeldChemical == null) return;

        costDisplay.RemoveCost(currentlyHeldChemical, currentlyHeldChemical.getCost());
        Destroy(currentlyHeldChemical.gameObject);
        SFXPlayer.instance.PlaySoundEffect(3);
    }

    public void TrashChem(HexTile tile)
    {
        if (tile.storedChemical == null) return;
        Chemical chem = tile.storedChemical;

        chem.UpdateNeighborsUponLeaving();
        costDisplay.RemoveCost(chem, chem.getCost());
        Destroy(chem.transform.gameObject);
        tile.storedChemical = null;
        tile.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public Chemical CreateChemChild(Chemical chemical, Vector2 location) //for multiple sized chemicals
    {
        if (currentlyHeldChemical == null) //weird stuff will happen if it's not held
        {
            HexTile hexLocation = hexGrid.GetHexTile(location);
            Chemical chem = Instantiate(childChem, hexLocation.transform.position, Quaternion.identity);
            chemical.SetChildConnections(chem);
            currentlyHeldChemical = chem;
            hexLocation.DropChem();
            return chem;
        }
        return null;
    }

    public Chemical CreateChemChild(Chemical chemical, HexTile location) //for multiple sized chemicals
    {
        if (currentlyHeldChemical == null) //weird stuff will happen if it's not held
        {
            Chemical chem = Instantiate(childChem, location.transform.position, Quaternion.identity);
            chemical.SetChildConnections(chem);
            currentlyHeldChemical = chem;
            location.DropChem();
            return chem;
        }
        return null;
    }

    public void MoveChildTile(Chemical chem, HexTile newTile)
    {
        chem.UpdateNeighborsUponLeaving();
        chem.ClearStatus();
        chem.housingTile.GetComponent<PolygonCollider2D>().enabled = true;
        chem.housingTile.storedChemical = null;

        chem.housingTile = newTile;
        newTile.GetComponent<PolygonCollider2D>().enabled = false;
        newTile.storedChemical = chem;
        chem.transform.position = newTile.transform.position; //this will cause problems with generated graphics
        chem.EvaluateConnections();
        foreach (SpriteRenderer SR in newTile.GetComponentsInChildren<SpriteRenderer>())
        {
            SR.sortingLayerName = "HexGraphics";
        }
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public void ResetTimeElapsed()
    {
        timeElapsed = 0;
        lastTimeStamp = 0;
        hours = 9;
        minutes = 0;
        qualifier = "AM";
    }

    public void StartRecapTutorial()
    {
        allrecapfinished = false;
        numrecapfinished = 0;
    }
}
