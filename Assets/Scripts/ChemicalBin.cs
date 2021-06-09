using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBin : MonoBehaviour
{

    // In the full development, there would be a "ChemicalBin" abstract class that this would inherit from
    public Chemical ChemicalPrefab;

    public void CreateDrug()
    {
        if (GameManager.instance.currentlyHeldChemical != null) return;

        Chemical newChemical = Instantiate<Chemical>(ChemicalPrefab);

        GameManager.instance.currentlyHeldChemical = newChemical;
        newChemical.isPlaced = false;
        newChemical.GetComponent<PolygonCollider2D>().enabled = false;

        foreach (SpriteRenderer SR in newChemical.GetComponentsInChildren<SpriteRenderer>())
        {
            SR.sortingLayerName = "LiftedTile";
        }
    }
}
