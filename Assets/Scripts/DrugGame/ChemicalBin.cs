using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBin : MonoBehaviour
{

    // In the full development, there would be a "ChemicalBin" abstract class that this would inherit from
    public Chemical ChemicalPrefab;
    public TMPro.TextMeshProUGUI title, costDisplay;
    public Transform graphicPosition;

    private void Start()
    {
        if (ChemicalPrefab != null)
        {
            if (title != null)
            {
                title.SetText(ChemicalPrefab.name);
            }
            if (costDisplay != null)
            {
                costDisplay.SetText("Cost " + ChemicalPrefab.getCost().ToString());
            }
            //CreateDrugGraphic();
        }
    }

    private void OnMouseDown()
    {
        if (!Printer.instance.solutionPrinted && ActiveContractArea.instance.currentContract != null)
            CreateDrug();
    }

    public void CreateDrug()
    {
        if (DrugManager.instance.currentlyHeldChemical != null) return;

        if (ChemicalPrefab != null)
        {
            Chemical newChemical = Instantiate<Chemical>(ChemicalPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            newChemical.name = ChemicalPrefab.name;//without this, it adds (clone) to the name which is annoying if you try to use them as IDs
            DrugManager.instance.currentlyHeldChemical = newChemical;
            newChemical.isPlaced = false;
            newChemical.GetComponent<PolygonCollider2D>().enabled = false;

            foreach (SpriteRenderer SR in newChemical.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "LiftedTile";
            }
        }
    }

    public void CreateDrugGraphic()
    {
        Chemical newChemical = Instantiate<Chemical>(ChemicalPrefab, graphicPosition.position, Quaternion.identity);
        GameObject graphic = Instantiate<GameObject>(newChemical.graphicsParent, graphicPosition.position, Quaternion.identity);
        graphic.transform.localScale = new Vector3(5, 5, 1);//CHANGE THIS WHEN GRAPHICS SCALE CHANGES

        Destroy(newChemical.gameObject);
    }

    public void ChangeCost(float cost)
    {
        costDisplay.SetText("Cost " + cost.ToString());
    }
}
