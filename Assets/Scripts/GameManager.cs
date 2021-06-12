using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Chemical currentlyHeldChemical;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f);
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Bin")
                {
                    ChemicalBin bin = hit.transform.GetComponent<ChemicalBin>();
                    if (bin != null)
                    {
                        bin.CreateDrug();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f);
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Grid")
                {
                    HexTile tile = hit.transform.GetComponent<HexTile>();
                    if (tile != null)
                    {
                        tile.DropChem();
                    }
                }

                if (hit.transform.gameObject.tag == "Trash")
                {
                    Garbage trash = hit.transform.GetComponent<Garbage>();
                    if (trash != null)
                    {
                        trash.TrashChem();
                    }
                }
            }
        }
    }
}
