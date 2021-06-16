using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private CostDisplay costDisplay;

    public Chemical currentlyHeldChemical;
    private Chemical lastHovered;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);


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
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.transform != null && hit.transform.gameObject.tag == "Rotate")
            {
                ChemicalRotateButton button = hit.transform.GetComponent<ChemicalRotateButton>();
                if (button != null)
                {
                    button.Rotate();
                }
            }
        }
        

        if (Input.GetMouseButton(0))
        {
            if (hit.transform != null)
            {
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
        else
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

    public void TrashChem()
    {
        if (currentlyHeldChemical == null) return;

        costDisplay.UpdateCost(currentlyHeldChemical.getCost() * -1);
        Destroy(currentlyHeldChemical.gameObject);
    }
}
