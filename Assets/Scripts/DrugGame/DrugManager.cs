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

    public Chemical currentlyHeldChemical;
    private Chemical lastHovered;
    private HexGrid hexGrid;

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
    }

    public void SetConnection(Chemical chemical, int index, string type)
    {
        chemical.connectionTypes[index] = type;
        chemical.EvaluateConnections();
    }

    public void TrashChem()
    {
        if (currentlyHeldChemical == null) return;

        costDisplay.UpdateCost(currentlyHeldChemical.getCost() * -1);
        Destroy(currentlyHeldChemical.gameObject);
    }

    public void TrashChem(HexTile tile)
    {
        if (tile.storedChemical == null) return;
        Chemical chem = tile.storedChemical;

        chem.UpdateNeighborsUponLeaving();
        costDisplay.UpdateCost(chem.getCost() * -1);
        Destroy(chem.transform.gameObject);
        tile.storedChemical = null;
    }

    public Chemical CreateChemChild(Chemical chemical, Vector2 location) //for multiple sized chemicals
    {
        if (currentlyHeldChemical == null) //weird stuff will happen if it's not held
        {
            HexTile hexLocation = hexGrid.GetHexTile(location);
            Chemical chem = Instantiate(childChem, hexLocation.transform.position, Quaternion.identity);
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
            currentlyHeldChemical = chem;
            location.DropChem();
            return chem;
        }
        return null;
    }
}
