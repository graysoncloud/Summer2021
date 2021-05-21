using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{
    [SerializeField]
    private string name;

    public bool isPlaced;
    public HexTile currentLocation;
    public GameObject chemicalObject;

    // Storage of the chemical's "bond" information. The first item in the array represents the top bond, then it proceeds clockwise.
    // 
    [SerializeField]
    private string TopConnection,
                   TRConnection,
                   BRConnection,
                   BottomConnection,
                   BLConnection,
                   TLConnection;

    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private ChemicalRotateButton leftButton,
                                rightButton;

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

    public void RotateConnections(float amount)
    {
        if (amount < 0)
        {
            // Clockwise rotation:
            string oldTop = TopConnection;
            TopConnection = TLConnection;
            TLConnection = BLConnection;
            BLConnection = BottomConnection;
            BottomConnection = BRConnection;
            BRConnection = TRConnection;
            TRConnection = oldTop;
        } else
        {
            // Counterclockwise rotation:
            string oldTop = TopConnection;
            TopConnection = TRConnection;
            TRConnection = BRConnection;
            BRConnection = BottomConnection;
            BottomConnection = BLConnection;
            BLConnection = TLConnection;
            TLConnection = oldTop;
        }
    }

    private string GetConnections()
    {
        return null;
    }
}
