using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{
    public float cost = 0;
    public EffectType[] effects;
    public int[] effectAmount;
    public bool desirable = false, undesirable = false;
    private int benefit = 0, detriment = 0, benefitAmount = 0, detrimentAmount = 0;
    private const float rotateSpeed = 15f;
    private float rotateTarget = 0, internalRotation = 0;

    public bool isPlaced, isChild = false;
    public bool amplified;
    public HexTile housingTile;

    // Storage of the chemical's "bond" information. The first item in the array represents the top bond, then it proceeds clockwise.
    //     Possible values: Positive, Negative, Neutral, Amplifer, (more to be added)
    public string[] connectionTypes = new string[6];

    // Store's the status of the chemical's bonds. As with the previous array, the first item is the topmost, then proceedcs clockwise.
    [SerializeField] 
    private string[] connectionStatuses = new string[6];

    private int childIndex1 = -1, childIndex2 = -1; //-1 = none
    public string[] childConnection1;
    public string[] childConnection2;

    public GameObject graphicsParent;
    private GameObject buttons;
    private ChemicalRotateButton leftButton = null,
                                rightButton = null;

    // Make sure prefabs are dragged in right order (see connectionTypesDict below)
    private GameObject[] connectionSprites;

    private VolatilityBar dangerBar;
    private BenefitValue benefitValue;
    private CostDisplay costDisplay;

    private SpriteRenderer spriteRenderer;

    
    Dictionary<string, int> connectionTypesDict = new Dictionary<string, int>() {
        { "None", 0 },
        { "Negative", 1},
        { "Neutral", 2},
        { "Positive", 3},
        { "Amplifier", 4},
        { "Chemical", 5 }
    };

    private void Awake()
    {
        // Saves us from having to assign redundant variables each time we create a new drug
        connectionSprites = GameObject.Find("ConnectionSpriteData").GetComponent<ConnectionSpriteData>().connectionSprites;
        if (!isChild)
        {
            graphicsParent = transform.Find("Graphics").gameObject;
            buttons = transform.Find("Buttons").gameObject;
            leftButton = buttons.transform.Find("RotateLeft").GetComponent<ChemicalRotateButton>();
            rightButton = buttons.transform.Find("RotateRight").GetComponent<ChemicalRotateButton>();
        }
        if (!isChild)
        {
            spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        }

        dangerBar = GameObject.FindObjectOfType<VolatilityBar>();
        benefitValue = GameObject.FindObjectOfType<BenefitValue>();
        costDisplay = GameObject.FindObjectOfType<CostDisplay>();

        for (int i = 0; i < 6; i++)
        {
            if (connectionTypes[i] == "Chemical")
            {
                if (childIndex1 == -1)
                    childIndex1 = i;
                else
                    childIndex2 = i;
            }
        }
        CreateConnections();
    }

    private void Start()
    {
        if (!isChild)
        {
            costDisplay.AddCost(this, cost);

            EffectType wanted = DrugManager.instance.GetDesireable();
            EffectType unwanted = DrugManager.instance.GetUndesireable();

            int i = 0;
            foreach (var effect in effects)
            {
                if (effect == wanted) //THIS SYSTEM DOESN'T WORK WELL WITH MULTIPLE DESIRABLE AND UNDESIRABLE
                {
                    desirable = true;
                    if (i + 1> effectAmount.Length) 
                        benefitAmount = 1;
                    else
                        benefitAmount = effectAmount[i];
                }
                    
                if (effect == unwanted)
                {
                    undesirable = true;
                    if (i + 1> effectAmount.Length) 
                        detrimentAmount = 1;
                    else
                        detrimentAmount = effectAmount[i];
                }
                i++;
            }
        }
    }

    public void CreateConnections()
    {
        if (!isChild)
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
                    if (connectionTypes[i] == "Chemical")
                        newConnection.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                }
                if (connectionTypes[i] == "Chemical")
                {
                    GameObject newConnection = Instantiate(connectionSprites[5]);//CHANGE IF MORE CONNECTIONS ARE ADDED
                    newConnection.GetComponent<SpriteRenderer>().sortingLayerName = "LiftedTile";
                    newConnection.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = spriteRenderer.color;

                    GameObject pivot = new GameObject("ConnectionPivot");
                    pivot.transform.localPosition = transform.position;
                    pivot.transform.SetParent(graphicsParent.transform);
                    newConnection.transform.localPosition = transform.position;
                    newConnection.transform.SetParent(pivot.transform);
                    newConnection.transform.Translate(new Vector3(0, 8.5088f, 0));
                    pivot.transform.Rotate(new Vector3(0, 0, -(60 * i)));

                    if (i == childIndex1)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (childConnection1[j] != "None")
                            {
                                GameObject childConnection = Instantiate(connectionSprites[connectionTypesDict[childConnection1[j]] - 1]);

                                // Instantiates new sprites with the assumption that the tile is lifted, because chemicals are always generated lifted
                                childConnection.GetComponent<SpriteRenderer>().sortingLayerName = "LiftedTile";

                                GameObject childPivot = new GameObject("ConnectionPivot");
                                childPivot.transform.localPosition = newConnection.transform.position;
                                childPivot.transform.SetParent(newConnection.transform);
                                childConnection.transform.localPosition = newConnection.transform.position;
                                childConnection.transform.SetParent(childPivot.transform);
                                childConnection.transform.Translate(new Vector3(0, offsetDist, 0));
                                childPivot.transform.Rotate(new Vector3(0, 0, -(60 * j)));
                                if (childConnection1[j] == "Chemical")
                                    childConnection.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                            }
                        }
                    } else if (i == childIndex2)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (childConnection2[j] != "None")
                            {
                                GameObject childConnection = Instantiate(connectionSprites[connectionTypesDict[childConnection2[j]] - 1]);

                                // Instantiates new sprites with the assumption that the tile is lifted, because chemicals are always generated lifted
                                childConnection.GetComponent<SpriteRenderer>().sortingLayerName = "LiftedTile";

                                GameObject childPivot = new GameObject("ConnectionPivot");
                                childPivot.transform.localPosition = newConnection.transform.position;
                                childPivot.transform.SetParent(newConnection.transform);
                                childConnection.transform.localPosition = newConnection.transform.position;
                                childConnection.transform.SetParent(childPivot.transform);
                                childConnection.transform.Translate(new Vector3(0, offsetDist, 0));
                                childPivot.transform.Rotate(new Vector3(0, 0, -(60 * j)));
                                if (childConnection2[j] == "Chemical")
                                    childConnection.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                            }
                        }
                    }
                    
                }
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

    private void FixedUpdate()
    {
        if (!isChild)
        {
            // Rotate stuff
            if (!Mathf.Approximately(internalRotation, rotateTarget))
            {
                float dir = rotateSpeed;
                float difference = Mathf.Abs(rotateTarget - internalRotation);
                if (rotateTarget < internalRotation)
                {
                    dir *= -1;
                }
                Mathf.Clamp(dir, -difference, difference);

                graphicsParent.transform.rotation = Quaternion.Euler(0, 0, graphicsParent.transform.rotation.eulerAngles.z + dir);
                internalRotation += dir;
            }
        }
    }

    private void OnMouseDown()
    {
        LiftChem();
    }

    public void RotateConnections(float amount)
    {
        if (amount % 60 != 0) return;
        if (!isChild)
        {
            Chemical child1 = null;
            Chemical child2 = null;

            if (isPlaced && childIndex1 != -1)
            {
                //check for occupied tiles before child rotation
                int oldChild1 = childIndex1;
                child1 = housingTile.neighbors[childIndex1].storedChemical;

                childIndex1 -= (int)amount / 60;
                if (childIndex1 == -1)
                    childIndex1 = 5;
                if (childIndex1 == 6)
                    childIndex1 = 0;
                HexTile adjacentTile = housingTile.neighbors[childIndex1];
                if (adjacentTile == null || adjacentTile.storedChemical != null)//needs an exception for adjacent multi chems
                {
                    childIndex1 = oldChild1;
                    return;
                }

                if (childIndex2 != -1)
                {
                    int oldChild2 = childIndex2;
                    child2 = housingTile.neighbors[childIndex2].storedChemical;

                    childIndex2 -= (int)amount / 60;
                    if (childIndex2 == -1)
                        childIndex2 = 5;
                    if (childIndex2 == 6)
                        childIndex2 = 0;
                    adjacentTile = this.housingTile.neighbors[childIndex2];
                    if (adjacentTile == null || adjacentTile.storedChemical != null)
                    {
                        childIndex1 = oldChild1;
                        childIndex2 = oldChild2;
                        return;
                    }
                    DrugManager.instance.MoveChildTile(child2, adjacentTile);
                }

                adjacentTile = housingTile.neighbors[childIndex1];
                DrugManager.instance.MoveChildTile(child1, adjacentTile);
            }

            rotateTarget += amount;

            connectionTypes = RotateArray(connectionTypes, amount);
            if (childIndex1 != -1)
            {
                childConnection1 = RotateArray(childConnection1, amount);
                if (isPlaced)
                {
                    child1.connectionTypes = (string[])childConnection1.Clone();
                    child1.EvaluateConnections();
                }
                else
                {
                    childIndex1 -= (int)amount / 60;
                    if (childIndex1 == -1)
                        childIndex1 = 5;
                    if (childIndex1 == 6)
                        childIndex1 = 0;
                }

                if (childIndex2 != -1)
                {
                    childConnection2 = RotateArray(childConnection2, amount);
                    if (isPlaced)
                    {
                        child2.connectionTypes = child2.connectionTypes = (string[])childConnection2.Clone();
                        child2.EvaluateConnections();
                    }
                    else
                    {
                        childIndex2 -= (int)amount / 60;
                        if (childIndex2 == -1)
                            childIndex2 = 5;
                        if (childIndex2 == 6)
                            childIndex2 = 0;
                    }
                }
            }


            if (DrugManager.instance.currentlyHeldChemical == null)
            {
                EvaluateConnections();
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (connectionTypes[i] == "Chemical")
                {
                    HexTile Parent = housingTile.neighbors[i];
                    Parent.storedChemical.RotateConnections(amount);
                    return;
                }
            }
        }
    }

    public string[] RotateArray(string[] array, float dir)
    {
        string[] newArray = new string[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            if (dir > 0)
            {
                if (i != array.Length - 1)
                    newArray[i] = array[i + 1];
                else
                    newArray[i] = array[0];
            }
            else
            {
                if (i != 0)
                    newArray[i] = array[i - 1];
                else
                    newArray[i] = array[array.Length - 1];
            }
        }
        return newArray;
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
        // Subscribes to the clear tile event
        DrugManager.onClearChems += TrashChem;

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
        UpdateBenefit();
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
                connectionStatuses[statusIndex] = "Neutral";
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
                connectionStatuses[statusIndex] = "Neutral";
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
                connectionStatuses[statusIndex] = "Neutral";
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
                amplified = true;
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }

        else if (connectionType == "Chemical")
        {
            connectionStatuses[statusIndex] = "None";
            AttemptSetStatus(statusIndex, adjacentTile, "None"); 
            // Can't set status since chemical doesn't exist yet
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
                amplified = true;
            }

            else
                Debug.LogError("Invalid connection type: " + connectionType);
        }

    }

    private void AttemptSetStatus(int statusIndex, HexTile adjacentTile, string status)
    {
        if (adjacentTile != null && adjacentTile.storedChemical != null && adjacentTile.storedChemical.GetConnectionTypeSingle((statusIndex + 3) % 6) != "None")
            adjacentTile.storedChemical.SetConnectionStatus((statusIndex + 3) % 6, status);
        if (status == "Amplified")
        {
            adjacentTile.storedChemical.amplified = true;
        }
    }

    public void UpdateBenefit()
    {
        UpdateBenefitNeighbor();
        for (int j = 0; j < 6; j++)
        {
            HexTile adjacentTile = this.housingTile.neighbors[j];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
                adjacentTile.storedChemical.UpdateBenefitNeighbor();
        }
    }

    public void UpdateBenefitNeighbor()
    {
        // call after statuses are changed
        if (desirable)
        {
            /*benefitValue.UpdateBenefitValue(-benefit);
            benefit = 0;
            foreach (string status in connectionStatuses)
            {
                if (status == "Positive")
                {
                    if (!amplified)
                        benefit++;
                    else
                        benefit += 2;
                }
            }
            if (!amplified)
                benefit++;
            else
                benefit += 2;*/
            benefitValue.UpdateBenefitValue(-benefit); // This code doesn't really make sense but it works
            benefit = benefitAmount;
            benefitValue.UpdateBenefitValue(benefit);
        }
        if (undesirable)
        {
            DrugManager.instance.undesiredChems -= detriment;
            detriment = detrimentAmount;
            DrugManager.instance.undesiredChems += detriment;
            ContractDisplayer.instance.UpdateBadEffect(DrugManager.instance.undesiredChems);
        }
    }

    public void ClearBenefit()
    {
        if (desirable)
        {
            benefitValue.UpdateBenefitValue(-benefitAmount);
            benefit = 0;
        }
        if (undesirable)
        {
            DrugManager.instance.undesiredChems -= detriment;
            detriment = 0;
            ContractDisplayer.instance.UpdateBadEffect(DrugManager.instance.undesiredChems);
        }
    }

    public void UpdateNeighborsUponLeaving()
    {
        // Called when a chemical is picked up; much simplified version of EvaluateConnections because results are limited to unstable or none

        DrugManager.onClearChems -= TrashChem;

        // For danger level / benefit value tracking
        string[] oldStatuses = new string[12];
        for (int i = 0; i < 6; i++)
        {
            oldStatuses[i] = connectionStatuses[i];

            HexTile adjacentTile = this.housingTile.neighbors[i];
            if (adjacentTile != null && adjacentTile.storedChemical != null)
            {
                oldStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
            }
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
                else if (connectionType == "None" || connectionType != "Amplifier")
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
            {
                newStatuses[i + 6] = adjacentTile.storedChemical.connectionStatuses[(i + 3) % 6];
                adjacentTile.storedChemical.CheckAmplified();
            }
            else
                newStatuses[i + 6] = "None";

        }

        dangerBar.UpdateDanger(oldStatuses, newStatuses);
        ClearBenefit();
        amplified = false;
    }

    public void LiftChem()
    {
        // Attempt to pick up the chemical, if another one is already being held
        if (DrugManager.instance.currentlyHeldChemical != null || leftButton != null && (leftButton.mouseOver || rightButton.mouseOver)) return;

        if (!this.isChild)
        {
            /*if (desirable)
                DrugManager.instance.desiredChems--;
            if (undesirable)
                DrugManager.instance.undesiredChems--;*/

            // Destroy children on pick up
            for (int i = 0; i < 6; i++)
            {
                if (connectionTypes[i] == "Chemical")
                {
                    HexTile childTile = this.housingTile.neighbors[i];
                    DrugManager.instance.TrashChem(childTile);
                }
            }


            // Update neighbors, and clear this chemical's statuses (alternatively, these could be returned to a default)
            UpdateNeighborsUponLeaving();
            connectionStatuses = new string[6];

            DrugManager.instance.currentlyHeldChemical = this;

            // Disable collisions for this
            this.GetComponent<PolygonCollider2D>().enabled = false;
            isPlaced = false;

            // Enable hexTile collisions (for mouse clicking)
            housingTile.GetComponent<PolygonCollider2D>().enabled = true;

            // "Raise" the tile's sprite above everything else
            foreach (SpriteRenderer SR in this.GetComponentsInChildren<SpriteRenderer>())
            {
                SR.sortingLayerName = "LiftedTile";
            }

            housingTile.storedChemical = null;
        } 
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (connectionTypes[i] == "Chemical")
                {
                    this.housingTile.neighbors[i].storedChemical.LiftChem();
                    return;
                }
            }
        }
    }

    public void SetChildConnections(Chemical child)
    {
        if (childIndex1 != -1)
        {
            child.connectionTypes = (string[]) childConnection1.Clone();
            //System.Array.Copy(childConnection1, housingTile.neighbors[childIndex1].storedChemical.connectionTypes, 6);
            if (childIndex2 != -1)
            {
                child.connectionTypes = (string[])childConnection2.Clone();
                //System.Array.Copy(childConnection2, housingTile.neighbors[childIndex2].storedChemical.connectionTypes, 6);
            }
        }
    }

    public void CheckAmplified()
    {
        amplified = false;
        for (int i = 0; i < 6; i++)
        {
            if (connectionStatuses[i] == "Amplified")
            {
                amplified = true;
            }
        }
        UpdateBenefitNeighbor();
    }

    public void TrashChem()
    {
        DrugManager.instance.TrashChem(housingTile);
    }

    public void ClearStatus()
    {
        connectionStatuses = new string [6];
    }

    public void setActive(bool active)
    {
        if (buttons != null)
            buttons.gameObject.SetActive(active);
    }

    public float getCost()
    {
        return cost;
    }

    public string getInfo()
    {

        var effectlist = new System.Text.StringBuilder();

        effectlist.AppendLine(name);

        
        for(int i = 0; i < effects.Length; i++){
            effectlist.AppendLine(effects[i].ToString() + " " + effectAmount[i].ToString());
        }
        
        return effectlist.ToString();
    }
}
