using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinManager : MonoBehaviour
{
    [SerializeField]
    private ChemicalBin bin;

    [SerializeField]
    private Chemical[] chemicals;
    // Could rework it so it loads from a json or something for different days

    [SerializeField]
    private float spacing = 0;

    private void Start()
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

            index++;
        }
    }
}
