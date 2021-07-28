using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSceneCamera : MonoBehaviour
{
    public static OfficeSceneCamera instance = null;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

}
