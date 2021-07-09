using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveContractArea : MonoBehaviour
{
    public static ActiveContractArea instance = null;

    public ContractFolder currentContract;
    public Vector3 contractSpot = new Vector3(0, 0, 0);

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
