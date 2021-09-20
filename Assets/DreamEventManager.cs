using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEventManager : MonoBehaviour
{
    public static DreamEventManager instance;


    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }


}
