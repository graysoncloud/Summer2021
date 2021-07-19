using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private DangerBar dangerBar;

    public int desiredChems = 0, undesiredChems = 0;

    public delegate void OnClearChems();
    public static event OnClearChems onClearChems;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        hexGrid = GameObject.FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
        dangerBar = GameObject.FindObjectOfType<DangerBar>();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f);  

        if (hit.transform != null) //hover over stuff
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
        if (currentlyHeldChemical != null)
        {
            if (Input.GetButtonDown("Rotate")) {
                float rotateDir = Input.GetAxis("Rotate");
                currentlyHeldChemical.RotateConnections(60 * -rotateDir);
            }
        }

        if (Input.GetMouseButtonDown(0))
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
        

        if (!Input.GetMouseButton(0))
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

    public EffectType GetDesireable()
    {
        return GameManager.instance.GetCurrentContract().desirableEffect;
    }

    public EffectType GetUndesireable()
    {
        return GameManager.instance.GetCurrentContract().undesirableEffect;
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
}
