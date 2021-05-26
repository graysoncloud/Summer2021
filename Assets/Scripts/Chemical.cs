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
    private string[] connectionTypes = new string[6];

    // Store's the status of the chemical's bonds. As with the previous array, the first item is the topmost, then proceedcs clockwise.
    [SerializeField]
    private string[] connectionStatuses = new string[6];

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

        UpdateNeighborsUponLeaving();
        connectionStatuses = new string[6];
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
            string oldTop = connectionTypes[0];
            connectionTypes[0] = connectionTypes[5];
            connectionTypes[5] = connectionTypes[4];
            connectionTypes[4] = connectionTypes[3];
            connectionTypes[3] = connectionTypes[2];
            connectionTypes[2] = connectionTypes[1];
            connectionTypes[1] = oldTop;
        } else
        {
            string oldTop = connectionTypes[0];
            connectionTypes[0] = connectionTypes[1];
            connectionTypes[1] = connectionTypes[2];
            connectionTypes[2] = connectionTypes[3];
            connectionTypes[3] = connectionTypes[4];
            connectionTypes[4] = connectionTypes[5];
            connectionTypes[5] = oldTop;
        }

        // Check if new connections have formed with neighbor / if old ones broke
        EvaluateConnections();
    }

    public string[] GetConnections()
    {
        return connectionTypes;
    }

    public string GetConnectionTypeSingle(int index)
    {
        return connectionTypes[index];
    }

    public void SetConnectionStatus(int index, string status)
    {
        connectionStatuses[index] = status;
    }

    public void EvaluateConnections()
    {
        // Determines the bonds formed between this chemical and its neighbors. The way its coded, each half of a bond is calculated
        //     independantly, and will contribute to the danger bar and benefit value on its own. This code updates the status of a
        //     chemical's bond, as well as the 6 adjacent bonds in its neighbors.

        // Run evaluateConnection 6 times, starting with the top and running clockwise
        EvaluateConnection(connectionTypes[0], 0, 0);
        EvaluateConnection(connectionTypes[1], 1, 1);
        EvaluateConnection(connectionTypes[2], 2, 2);
        EvaluateConnection(connectionTypes[3], 3, 3);
        EvaluateConnection(connectionTypes[4], 4, 4);
        EvaluateConnection(connectionTypes[5], 5, 5);

        // Update score mechanism, maybe run a function in a scorekeeper UI
    }

    private void EvaluateConnection(string connectionType, int statusIndex, int adjacentIndex)
    {
        HexTile adjacentTile;
        string adjacentConnectionType;

        adjacentTile = this.housingTile.neighbors[adjacentIndex];

        // Find the complementary bond in the adjacent chemical, or set it to null if it doesn't exist
        if (adjacentTile != null && adjacentTile.storedChemical != null)
        {
            adjacentConnectionType = adjacentTile.storedChemical.GetConnectionTypeSingle((adjacentIndex + 3) % 6);
            // vDebug.Log("adjcanet connection type: " + adjacentConnectionType);
        }
        else
            adjacentConnectionType = null;


        // Off a positive connection, the two possible outcomes are unstable and positive (?)
        if (connectionType == "Positive")
        {
            // Unstable conditions (no neighbor, adjacent bond is wrong type)
            // NOTE: This will likely change as design is altered
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Positive" && adjacentConnectionType != "Neutral"))
            {
                //Debug.Log((adjacentConnectionType != "Positive") + ", " + (adjacentConnectionType != "Neutral")); 
                connectionStatuses[statusIndex] = "Unstable";
                // Small calculation to reverse the index on the neighbor hexTile
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Unstable");

                // Is there anything more to do?
            }

            // Success conditions (coresponding positive or neutral connection)
            else if (adjacentConnectionType == "Positive" || adjacentConnectionType == "Neutral")
            {
                // Something other than "Positive" may be more descriptive
                connectionStatuses[statusIndex] = "Positive";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Positive");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a negative connection, the two possible outcomes are unstable and negative (?)
        else if (connectionType == "Negative")
        {
            // Unstable conditions:
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Negative" && adjacentConnectionType != "Neutral"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Unstable");
            }

            // Success conditions (coresponding positive or neutral connection):
            else if (adjacentConnectionType == "Negative" || adjacentConnectionType == "Neutral")
            {
                connectionStatuses[statusIndex] = "Negative";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Negative");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a neutral connection, the three possible outcomes are unstable, positive and negative (?)
        else if (connectionType == "Neutral")
        {
            // Unstable conditions: (as it is, two neutral connections make an unstable bond)
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Negative" && adjacentConnectionType != "Positive"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Unstable");
            }

            // Success conditions (coresponding positive or neutral connection):
            else if (adjacentConnectionType == "Negative" || adjacentConnectionType == "Positive")
            {
                // If the names for connection statuses are changed, this will be buggy. But it works as is.
                connectionStatuses[statusIndex] = adjacentConnectionType;
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, adjacentConnectionType);
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a amplifer connection, the two possible outcomes are unstable and amplifying
        else if (connectionType == "Amplifier")
        {
            // Unstable conditions: (no conflicts if neighbor if an amplifier or a supressor)
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "None" && adjacentConnectionType != "Amplifier"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Unstable");
            }

            // Success condition 1 (one amplification):
            else if (adjacentConnectionType == "None")
            {
                // The amplifier will never have a status, but it will change a "None" connection's status to "Amplified" so we cab track what's amplified
                connectionStatuses[statusIndex] = "None";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Amplified");
            }

            // Success condition 2 (two amplifications):
            else if (adjacentConnectionType == "Amplifier")
            {
                connectionStatuses[statusIndex] = "Amplified";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Amplified");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a "None" connection, the two possible outcomes are unstable and amplifying
        else if (connectionType == "None")
        {
            // Nothing conditions:
            if (adjacentTile == null || adjacentTile.storedChemical == null || adjacentConnectionType == "None")
            {
                connectionStatuses[statusIndex] = "None";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "None");
            }

            // Unstable conditions: (no conflicts if neighbor if an amplifier or a supressor)
            else if ((adjacentConnectionType != "None" && adjacentConnectionType != "Amplifier"))
            {
                connectionStatuses[statusIndex] = "None";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "Unstable");
            }

            // Amplification condition:
            else if (adjacentConnectionType == "Amplifier")
            {
                connectionStatuses[statusIndex] = "Amplified";
                if (adjacentTile != null && adjacentTile.storedChemical != null)
                    adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, "None");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }

    }

    // Called when a chemical is picked up; updates all surrounding connections
    public void UpdateNeighborsUponLeaving()
    {
        for (int i = 0; i < 6; i++)
        {
            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
            {
                string connectionType = adjacentTile.storedChemical.GetConnectionTypeSingle((i + 3) % 6);
                if (connectionType != "None" && connectionType != "Amplifier")
                {
                    adjacentTile.storedChemical.SetConnectionStatus((i + 3) % 6, "Unstable");
                }
                else if (connectionType == "None" || connectionType == "Amplifier")
                {
                    adjacentTile.storedChemical.SetConnectionStatus((i + 3) % 6, "None");
                }
            }
        }
    }

}
