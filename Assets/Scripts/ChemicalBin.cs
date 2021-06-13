using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBin : MonoBehaviour
{

    // In the full development, there would be a "ChemicalBin" abstract class that this would inherit from
    public Chemical ChemicalPrefab;
    private TMPro.TextMeshProUGUI title;

    private void Start()
    {
        title = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (ChemicalPrefab != null)
        {
            title.SetText(ChemicalPrefab.name);
        }
        
    }

    public void CreateDrug()
    {
        if (GameManager.instance.currentlyHeldChemical != null) return;

        if (ChemicalPrefab != null)
        {
            Chemical newChemical = Instantiate<Chemical>(ChemicalPrefab);
            newChemical.name = ChemicalPrefab.name;//without this, it adds (clone) to the name which is annoying if you try to use them as IDs

            GameManager.instance.currentlyHeldChemical = newChemical;
            newChemical.isPlaced = false;
            newChemical.GetComponent<PolygonCollider2D>().enabled = false;

            foreach (SpriteRenderer SR in newChemical.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "LiftedTile";
            }
        }
    }
}
