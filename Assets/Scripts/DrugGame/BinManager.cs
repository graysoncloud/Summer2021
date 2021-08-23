using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinManager : MonoBehaviour
{
    [SerializeField]
    private ChemicalBin bin = null;
    
    [SerializeField]
    public Chemical[] chemicals = null;
    // Could rework it so it loads from a json or something for different days
    public List<ChemicalBin> binArray = new List<ChemicalBin>(); //for getting bins from chem

    [SerializeField]
    private float spacing = 0;

    [SerializeField]
    private GameObject InfoDisplayer;  
    private void Awake()
    {
        int index = 0;
        foreach (Chemical chem in chemicals)
        {
            ChemicalBin chemBin = Instantiate(bin);
            chemBin.transform.SetParent(this.transform, false);
            Vector3 pos = this.transform.position;

            //I could get the scale and multiply it by the height but it's unnecessary for the finished assets
            pos.y -= index * (spacing);

            chemBin.ChemicalPrefab = chem;
            chemBin.transform.position = pos;

            chemBin.InfoTooltip = InfoDisplayer;

            binArray.Add(chemBin);
            index++;
        }
    }

    public ChemicalBin GetBin(Chemical chemical)
    {
        foreach (ChemicalBin bin in binArray)
        {
            if (bin.ChemicalPrefab.name == chemical.name)
            {
                return bin;
            }
        }
        Debug.LogWarning("Chemical bin not found");
        return null;
    }

    public Chemical[] GetChemicals()
    {
        return chemicals;
    }
}
