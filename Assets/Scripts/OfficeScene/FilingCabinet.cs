using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilingCabinet : MonoBehaviour
{
    public static FilingCabinet instance = null;

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
