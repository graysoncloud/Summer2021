using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{
    [SerializeField]
    private string name;

    public bool isPlaced;
    public HexTile housingTile;
    public GameObject chemicalObject;

    // Storage of the chemical's "bond" information. The first item in the array represents the top bond, then it proceeds clockwise.
    //     Possible values: Positive, Negative, Neutral, Amplifer, (more to be added)
    [SerializeField]
    private string TopConnectionType,
                   TRConnectionType,
                   BRConnectionType,
                   BottomConnectionType,
                   BLConnectionType,
                   TLConnectionType;

    // Store's the status of the chemical's bonds
    //     Possible values: Positive
    [SerializeField]
    private string TopConnectionStatus,
                   TRConnectionStatus,
                   BRConnectionStatus,
                   BottomConnectionStatus,
                   BLConnectionStatus,
                   TLConnectionStatus;

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
        // Attempt to pick up the chemical, if another one is already being held
        if (GameManager.instance.currentlyHeldChemical != null) return;

        GameManager.instance.currentlyHeldChemical = this;

        // Disable collisions for this
        this.GetComponent<PolygonCollider2D>().enabled = false;
        isPlaced = false;

        // Enable hexTile collisions (for mouse clicking)
        housingTile.GetComponent<PolygonCollider2D>().enabled = true;

        // "Raise" the tile's sprite above everything else
        foreach(SpriteRenderer SR in chemicalObject.GetComponentsInChildren<SpriteRenderer>())
        {
            SR.sortingLayerName = "LiftedTile";
        }

        housingTile.storedChemical = null;
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
            string oldTop = TopConnectionType;
            TopConnectionType = TLConnectionType;
            TLConnectionType = BLConnectionType;
            BLConnectionType = BottomConnectionType;
            BottomConnectionType = BRConnectionType;
            BRConnectionType = TRConnectionType;
            TRConnectionType = oldTop;
        } else
        {
            // Counterclockwise rotation:
            string oldTop = TopConnectionType;
            TopConnectionType = TRConnectionType;
            TRConnectionType = BRConnectionType;
            BRConnectionType = BottomConnectionType;
            BottomConnectionType = BLConnectionType;
            BLConnectionType = TLConnectionType;
            TLConnectionType = oldTop;
        }

        // Check if new connections have formed with neighbor / if old ones broke
    }

    public string[] GetConnections()
    {
        string[] toReturn = new string[6];
        toReturn[0] = TopConnectionType;
        toReturn[1] = TRConnectionType;
        toReturn[2] = BRConnectionType;
        toReturn[3] = BottomConnectionType;
        toReturn[4] = BLConnectionType;
        toReturn[5] = TLConnectionType;

        return toReturn; 
    }

    public void EvaluateConnections()
    {
        // Determines the bonds formed between this chemical and its neighbors. The way its coded, each half of a bond is calculated
        //     independantly, and will contribute to the danger bar and benefit value on its own. This code updates the status of a
        //     chemical's bond, as well as the 6 adjacent bonds in its neighbors.

        // Run evaluateConnection 6 times
    }

    private void EvaluateConnection(string connectionType, string connectionLocation, ref string connectionStatus)
    {
        HexTile adjacentTile;
        int adjacentIndex;
        string adjacentConnectionType;

        // Check to see if the neighbor exists
        switch (connectionLocation)
        {
            case "Top":
                adjacentIndex = 0;
                break;
            case "TR":
                adjacentIndex = 1;
                break;
            case "BR":
                adjacentIndex = 2;
                break;
            case "Bottom":
                adjacentIndex = 3;
                break;
            case "BL":
                adjacentIndex = 4;
                break;
            case "TL":
                adjacentIndex = 5;
                break;
            default:
                adjacentIndex = -1;
                Debug.LogError("Invalid Connection Type: " + connectionType);
                break;
        }
        adjacentTile = this.housingTile.neighbors[adjacentIndex];

        // Find the complementary bond in the adjacent chemical
        if (adjacentTile != null)
        {
            switch (connectionLocation)
            {
                case "Top":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("Bottom");
                    break;
                case "TopR":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("BL");
                    break;
                case "BottomR":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("TL");
                    break;
                case "Bottom":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("Top");
                    break;
                case "BottomL":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("TR");
                    break;
                case "TopL":
                    adjacentConnectionType = adjacentTile.storedChemical.GetConnectionType("BR");
                    break;
                default:
                    Debug.LogError("Invalid Connection Location: " + connectionLocation);
                    break;
            }
        }

        if (connectionType == "Postive")
        {
            // Unstable conditions (no neighbor, adjacent bond is wrong type)
            // NOTE: This will likely change as design is altered
            if (adjacentTile == null || adjacentTile) {
            }
        }

    }

    public string GetConnectionType(string location)
    {
        switch (location)
        {
            case "Top":
                return TopConnectionType;
            case "TR":
                return TRConnectionType;
            case "BR":
                return BRConnectionType;
            case "Bottom":
                return BottomConnectionType;
            case "BL":
                return BLConnectionType;
            case "TL":
                return TLConnectionType;
            default:
                Debug.LogError("Invalid Connection Location: " + location);
                return null;
        }
    }
}
