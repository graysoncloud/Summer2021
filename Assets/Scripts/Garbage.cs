using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private Color defaultColor = new Color(1f, 1f, 1f, 1f);
    private Color highlightColor = new Color(1f, .63f, .63f, 1f);

    [SerializeField]
    private CostDisplay costDisplay;

    private void OnMouseOver()
    {
        this.GetComponent<SpriteRenderer>().color = highlightColor;
    }

    private void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void OnMouseDown()
    {
        TrashChem();
    }

    //On mouse up doesn't work with holding chems, so it's done in game manager

    public void TrashChem()
    {
        if (GameManager.instance.currentlyHeldChemical == null) return;

        costDisplay.UpdateCost(GameManager.instance.currentlyHeldChemical.getCost() * -1);
        Destroy(GameManager.instance.currentlyHeldChemical.gameObject);
        Debug.Log(GameManager.instance.currentlyHeldChemical);
    }
}
