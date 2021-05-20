using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chemical : MonoBehaviour
{
    public string name;
    public bool isPlaced;
    public HexTile currentLocation;
    public GameObject chemicalObject;

    public GameObject buttons;
    public ChemicalRotateButton leftButton;
    public ChemicalRotateButton rightButton;

    private void Update()
    {
        // May have to do things if it is placed, in which case change this structure
        if (isPlaced) return;

        Vector3 toMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        toMoveTo.z = 0;
        transform.position = toMoveTo;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.currentlyHeldChemical != null) return;

        GameManager.instance.currentlyHeldChemical = this;

        // Disable collisions for this
        this.GetComponent<PolygonCollider2D>().enabled = false;
        isPlaced = false;

        // Enable hexTile collisions
        currentLocation.GetComponent<PolygonCollider2D>().enabled = true;

        // "Raise" the tile above everything else
        foreach(SpriteRenderer SR in chemicalObject.GetComponentsInChildren<SpriteRenderer>())
        {
            SR.sortingLayerName = "LiftedTile";
        }

        currentLocation.storedChemical = null;
    }

    private void OnMouseOver()
    {
        if (GameManager.instance.currentlyHeldChemical != null) return;
        buttons.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        StartCoroutine("TurnOffButtons");
    }

    IEnumerator TurnOffButtons()
    {
        yield return new WaitForEndOfFrame();
        if (!(leftButton.mouseOver || rightButton.mouseOver))
            buttons.gameObject.SetActive(false);
    }
}
