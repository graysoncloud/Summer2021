using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    private int height,
                width;

    [SerializeField]
    private GameObject hexRow;
    [SerializeField]
    private HexTile hexTile;

    private Dictionary<Vector2, HexTile> tileDictionary = new Dictionary<Vector2, HexTile>();  

    private void Start()
    {
        // Generating the grid
        for (int i = 0; i < height; i++)
        {
            // On alternating rows, increases number of hexTiles by 1. First row should have fewer, but this can be changed
            int tempWidth = Mathf.FloorToInt(width / 2) + (i % 2);

            // Row objects are primarily for laying the grid out (horizontal layout groups)
            GameObject newRow = Instantiate(hexRow);
            newRow.transform.SetParent(this.transform);

            for (int j = 0; j < tempWidth; j++)
            {
                HexTile newTile = Instantiate(hexTile);
                newTile.transform.SetParent(newRow.transform);

                Vector2 coordinates = new Vector2(j * 2 + ((i + 1) % 2), height - (i + 1));
                //Debug.Log("(" + (tempWidth - j) + ", " + (height - i) + ")");

                tileDictionary.Add(coordinates, newTile);
                newTile.SetCoordinates(coordinates);
            }

        }

        foreach(KeyValuePair<Vector2, HexTile> entry in tileDictionary)
        {
            entry.Value.neighbors = GetAdjacent(entry.Key);
        } 

    }

    public HexTile[] GetAdjacent(Vector2 startingPoint)
    {
        // NOTE: For multi-hex chemicals, I'm currently thinking the best bet would be to run a function
        //     like this on each individual hexTile it occupies. Are there better solutions though?
        
        HexTile[] returnArray = new HexTile[6];
        HexTile value;

        // Starts at the top, rotating clockwise
        Vector2 toAdd = new Vector2(startingPoint.x, startingPoint.y + 2);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[0] = value;

        toAdd = new Vector2(startingPoint.x + 1, startingPoint.y + 1);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[1] = value;

        toAdd = new Vector2(startingPoint.x + 1, startingPoint.y - 1);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[2] = value;

        toAdd = new Vector2(startingPoint.x, startingPoint.y - 2);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[3] = value;

        toAdd = new Vector2(startingPoint.x - 1, startingPoint.y - 1);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[4] = value;

        toAdd = new Vector2(startingPoint.x - 1, startingPoint.y + 1);
        if (tileDictionary.TryGetValue(toAdd, out value))
            returnArray[5] = value;

        return returnArray;
    }

}
