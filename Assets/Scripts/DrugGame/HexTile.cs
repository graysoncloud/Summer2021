using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Vector2 coordinates;

    public Chemical storedChemical;

    // Neighbors is a 6-long array of a hextile's neighbors, starting with the topmost one and rotating clockwise
    public HexTile[] neighbors;

    private Color mouseOverColor = new Color(.6f, .87f, .9f, 1f);
    private Color defaultColor = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        spriteRenderer.color = mouseOverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        DropChem();
    }

    private void OnMouseUp()
    {
        DropChem();
    }

    public void DropChem()
    {
        // Deposit the currently held chemical, if one exists and there isn't already one here
        if (DrugManager.instance.currentlyHeldChemical != null && storedChemical == null)
        {
            storedChemical = DrugManager.instance.currentlyHeldChemical;

            string[] connections = storedChemical.GetConnections();
            for (int i = 0; i < 6; i++)
            {
                if (connections[i] == "Chemical")
                {
                    if (neighbors[i] == null)
                    {
                        DrugManager.instance.TrashChem();
                        return;
                    }
                    if (neighbors[i].storedChemical != null)
                    {
                        return;
                    }
                    DrugManager.instance.currentlyHeldChemical = null;
                    DrugManager.instance.CreateChemChild(storedChemical, neighbors[i]);
                }
            }

            storedChemical.GetComponent<PolygonCollider2D>().enabled = true;
            this.GetComponent<PolygonCollider2D>().enabled = false;
            storedChemical.isPlaced = true;
            storedChemical.housingTile = this;
            storedChemical.transform.position = transform.position;

            foreach (SpriteRenderer SR in storedChemical.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "HexGraphics";
            }

            storedChemical.EvaluateConnections();

            DrugManager.instance.currentlyHeldChemical = null;
        }
    }

    public void SetCoordinates(Vector2 coords)
    {
        this.coordinates = coords;
    }
}
