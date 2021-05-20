using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Chemical storedChemical;

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

        // Deposit the currently held chemical, if one exists and there isn't already one here
        if (GameManager.instance.currentlyHeldChemical != null && storedChemical == null)
        {
            // Are there any problems with this sequence?
            storedChemical = GameManager.instance.currentlyHeldChemical;
            storedChemical.GetComponent<PolygonCollider2D>().enabled = true;
            this.GetComponent<PolygonCollider2D>().enabled = false;
            storedChemical.isPlaced = true;
            storedChemical.currentLocation = this;
            storedChemical.transform.position = transform.position;

            foreach (SpriteRenderer SR in storedChemical.chemicalObject.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "HexGraphics";
            }

            GameManager.instance.currentlyHeldChemical = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
