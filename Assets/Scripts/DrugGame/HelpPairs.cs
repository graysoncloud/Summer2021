using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPairs : MonoBehaviour
{
    public GameObject leftSide;
    public GameObject rightSide;
    public string[] leftConnection;
    public string[] rightConnection;
    private int currentConnectionShowing;
    private Color positive = new Color(0.5528828f, 0.9150943f, 0.5136614f, 1f);
    private Color negative = new Color(0.9137255f, 0.5339166f, 0.5137255f, 1f);
    private Color neutral = new Color(0.9137255f, 0.8064089f, 0.5137255f, 1f);
    private Color none = new Color(0f, 0f, 0f, 0f);
    private float timeElapsed;

    Dictionary<string, Color> connectionColors = new Dictionary<string, Color>() {};

    // Start is called before the first frame update
    void Start()
    {
        connectionColors["neutral"] = neutral;
        connectionColors["positive"] = positive;
        connectionColors["negative"] = negative;
        connectionColors["none"] = none;

        if(leftConnection.Length != 0){
            currentConnectionShowing = 0;
            leftSide.GetComponent<SpriteRenderer>().color = connectionColors[leftConnection[currentConnectionShowing]];
            rightSide.GetComponent<SpriteRenderer>().color = connectionColors[rightConnection[currentConnectionShowing]];
        }
    }

    public void Update()
    {
        //TODO: ALTERNATE BETWEEN COLORS EVERY LIKE 1 SECOND OR SOMETHING
    }
    public void NextSet()
    {
        currentConnectionShowing++;
        if(currentConnectionShowing >= leftConnection.Length)
        {
            currentConnectionShowing = 0;
        }
        leftSide.GetComponent<SpriteRenderer>().color = connectionColors[leftConnection[currentConnectionShowing]];
        rightSide.GetComponent<SpriteRenderer>().color = connectionColors[rightConnection[currentConnectionShowing]];
    }

    public void PreviousSet()
    {
        currentConnectionShowing--;
        if(currentConnectionShowing < 0)
        {
            currentConnectionShowing = leftConnection.Length - 1;
        }
        leftSide.GetComponent<SpriteRenderer>().color = connectionColors[leftConnection[currentConnectionShowing]];
        rightSide.GetComponent<SpriteRenderer>().color = connectionColors[rightConnection[currentConnectionShowing]];
    }
}
