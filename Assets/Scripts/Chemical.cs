using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    private float cost;

    public bool isPlaced;
    public HexTile housingTile;

    // Storage of the chemical's "bond" information. The first item in the array represents the top bond, then it proceeds clockwise.
    //     Possible values: Positive, Negative, Neutral, Amplifer, (more to be added)
    [SerializeField]
    private string[] connectionTypes = new string[6];

    // Store's the status of the chemical's bonds. As with the previous array, the first item is the topmost, then proceedcs clockwise.
    [SerializeField]
    private string[] connectionStatuses = new string[6];

    private GameObject graphicsParent;
    private GameObject buttons;
    private ChemicalRotateButton leftButton,
                                rightButton;

    // Make sure prefabs are dragged in right order (see connectionTypesDict below)
    private GameObject[] connectionSprites;
    [SerializeField]
    private ConnectionSpriteData ConnectionSpriteData;

    private DangerBar dangerBar;
    private BenefitValue benefitValue;
    private CostDisplay costDisplay;

    
    Dictionary<string, int> connectionTypesDict = new Dictionary<string, int>() {
        { "None", 0 },
        { "Negative", 1},
        { "Neutral", 2},
        { "Positive", 3},
        { "Amplifier", 4}
    };

    private void Start()
    {
        // Saves us from having to assign redundant variables each time we create a new drug
        connectionSprites = GameObject.Find("ConnectionSpriteData").GetComponent<ConnectionSpriteData>().connectionSprites;
        graphicsParent = transform.Find("Graphics").gameObject;
        buttons = transform.Find("Buttons").gameObject;
        leftButton = buttons.transform.Find("RotateLeft").GetComponent<ChemicalRotateButton>();
        rightButton = buttons.transform.Find("RotateRight").GetComponent<ChemicalRotateButton>();
        dangerBar = GameObject.FindObjectOfType<DangerBar>();
        benefitValue = GameObject.FindObjectOfType<BenefitValue>();
        costDisplay = GameObject.FindObjectOfType<CostDisplay>();

        costDisplay.UpdateCost(cost);

        CreateConnections();
    }

    public void CreateConnections()
    {
        // create the visual elements for the connections
        float offsetDist = 2.7f;
        for (int i = 0; i < 6; i++)
        {
            if (connectionTypes[i] != "None")
            {
                GameObject newConnection = Instantiate(connectionSprites[connectionTypesDict[connectionTypes[i]] - 1]);

                // Instantiates new sprites with the assumption that the tile is lifted, because chemicals are always generated lifted
                newConnection.GetComponent<SpriteRenderer>().sortingLayerName = "LiftedTile";

                GameObject pivot = new GameObject("ConnectionPivot");
                pivot.transform.localPosition = transform.position;
                pivot.transform.SetParent(graphicsParent.transform);
                newConnection.transform.localPosition = transform.position;
                newConnection.transform.SetParent(pivot.transform);
                newConnection.transform.Translate(new Vector3(0, offsetDist, 0));
                pivot.transform.Rotate(new Vector3(0, 0, -(60 * i)));
            }
        }
    }


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

        // Update neighbors, and clear this chemical's statuses (alternatively, these could be returned to a default)
        UpdateNeighborsUponLeaving();
        connectionStatuses = new string[6];

        GameManager.instance.currentlyHeldChemical = this;

        // Disable collisions for this
        this.GetComponent<PolygonCollider2D>().enabled = false;
        isPlaced = false;

        // Enable hexTile collisions (for mouse clicking)
        housingTile.GetComponent<PolygonCollider2D>().enabled = true;

        // "Raise" the tile's sprite above everything else
        foreach(SpriteRenderer SR in this.GetComponentsInChildren<SpriteRenderer>())
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
        // Bit buggy right now. Delay is needed to prevent a flashing effect
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

        // Changes to the danger level* / benefit value* are calculated by comparing the starting and ending statuses of this chemical and its neighbors.
        //     Indexes 0 - 5 are this chemical's old statuses, and indexes 6 - 11 are its neighbor's old statuses, starting at the top and rotating clockwise
        string[] oldStatuses = new string[12];
        for (int i = 0; i < 6; i ++)
        {
            oldStatuses[i] = connectionStatuses[i];

            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
                oldStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
            else
                // None it technically a status, whereas this is the absence of a status, but in this case they both represent an absence of danger / benefit
                oldStatuses[i + 6] = "None";
        }

        // Run evaluateConnection 6 times, starting with the top and running clockwise
        EvaluateConnection(connectionTypes[0], 0, 0);
        EvaluateConnection(connectionTypes[1], 1, 1);
        EvaluateConnection(connectionTypes[2], 2, 2);
        EvaluateConnection(connectionTypes[3], 3, 3);
        EvaluateConnection(connectionTypes[4], 4, 4);
        EvaluateConnection(connectionTypes[5], 5, 5);

        // Create second array of statuses to compare against old ones
        string[] newStatuses = new string[12];
        for (int i = 0; i < 6; i++)
        {
            newStatuses[i] = connectionStatuses[i];

            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
                newStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
            else
                newStatuses[i + 6] = "None";
        }

        // Update score (could this be run in the scorekeeper's script?)
        dangerBar.UpdateDanger(oldStatuses, newStatuses);
        benefitValue.UpdateBenefitValue(oldStatuses, newStatuses);
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
            // Debug.Log("adjcanet connection type: " + adjacentConnectionType);
        }
        else
            adjacentConnectionType = null;


        // Off a positive connection, the two possible outcomes are unstable and positive
        if (connectionType == "Positive")
        {
            // Unstable conditions (no neighbor, adjacent bond is wrong type)
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Positive" && adjacentConnectionType != "Neutral"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                AttemptSetStatus(statusIndex, adjacentTile, "Unstable");

                // Is there anything more to do?
            }

            // Positive conditions:
            else if (adjacentConnectionType == "Positive")
            {
                // Something other than "Positive" may be more descriptive
                connectionStatuses[statusIndex] = "Positive";
                AttemptSetStatus(statusIndex, adjacentTile, "Positive");
            }

            // Neutral conditions:
            else if (adjacentConnectionType == "Neutral")
            {
                connectionStatuses[statusIndex] = "Positive";
                AttemptSetStatus(statusIndex, adjacentTile, "Neutral");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a negative connection, the two possible outcomes are unstable and negative
        else if (connectionType == "Negative")
        {
            // Unstable conditions:
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Negative" && adjacentConnectionType != "Neutral"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                AttemptSetStatus(statusIndex, adjacentTile, "Unstable");
            }

            // Negative conditions:
            else if (adjacentConnectionType == "Negative")
            {
                connectionStatuses[statusIndex] = "Negative";
                AttemptSetStatus(statusIndex, adjacentTile, "Negative");
            }

            // Neutral conditions:
            else if (adjacentConnectionType == "Neutral")
            {
                connectionStatuses[statusIndex] = "Negative";
                AttemptSetStatus(statusIndex, adjacentTile, "Neutral");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }


        // Off a neutral connection, the three possible outcomes are unstable, positive and negative
        else if (connectionType == "Neutral")
        {
            // Unstable conditions: (as it is, two neutral connections make an unstable bond)
            if (adjacentTile == null || adjacentTile.storedChemical == null || (adjacentConnectionType != "Negative" && adjacentConnectionType != "Positive"))
            {
                connectionStatuses[statusIndex] = "Unstable";
                AttemptSetStatus(statusIndex, adjacentTile, "Unstable");
            }

            // Success conditions (coresponding positive or neutral connection):
            else if (adjacentConnectionType == "Negative" || adjacentConnectionType == "Positive")
            {
                // If the names for connection statuses are changed, this will be buggy. But it works as is.
                connectionStatuses[statusIndex] = adjacentConnectionType;
                AttemptSetStatus(statusIndex, adjacentTile, "Neutral");
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
                AttemptSetStatus(statusIndex, adjacentTile, "Unstable");
            }

            // Single amplification condition:
            else if (adjacentConnectionType == "None")
            {
                // The amplifier will never have a status, but it will change a "None" connection's status to "Amplified" so we cab track what's amplified
                connectionStatuses[statusIndex] = "None";
                AttemptSetStatus(statusIndex, adjacentTile, "Amplified");
            }

            // Double amplification condition:
            else if (adjacentConnectionType == "Amplifier")
            {
                connectionStatuses[statusIndex] = "Amplified";
                AttemptSetStatus(statusIndex, adjacentTile, "Amplified");
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
                AttemptSetStatus(statusIndex, adjacentTile, "None");
            }

            // Unstable conditions: (no conflicts if neighbor if an amplifier or a supressor)
            else if ((adjacentConnectionType != "None" && adjacentConnectionType != "Amplifier"))
            {
                connectionStatuses[statusIndex] = "None";
                AttemptSetStatus(statusIndex, adjacentTile, "Unstable");
            }

            // Single amplification condition:
            else if (adjacentConnectionType == "Amplifier")
            {
                connectionStatuses[statusIndex] = "Amplified";
                AttemptSetStatus(statusIndex, adjacentTile, "None");
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }

    }

    private void AttemptSetStatus(int statusIndex, HexTile adjacentTile, string status)
    {
        if (adjacentTile != null && adjacentTile.storedChemical != null)
            adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, status);
    }

    public void UpdateNeighborsUponLeaving()
    {
        // Called when a chemical is picked up; much simplified version of EvaluateConnections because results are limited to unstable or none

        // For danger level / benefit value tracking
        string[] oldStatuses = new string[12];
        for (int i = 0; i < 6; i++)
        {
            oldStatuses[i] = connectionStatuses[i];

            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
                oldStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
            else
                oldStatuses[i + 6] = "None";
        }

        // Status changing
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

        // Recalculate statuses 
        string[] newStatuses = new string[12];
        for (int i = 0; i < 6; i++)
        {
            // This tile was just removed, so all its connections no longer exist
            newStatuses[i] = "None";

            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
                newStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
            else
                newStatuses[i + 6] = "None";
        }

        dangerBar.UpdateDanger(oldStatuses, newStatuses);
    }

    public float getCost()
    {
        return cost;
    }

}
