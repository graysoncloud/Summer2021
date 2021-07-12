using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapSceneManager : MonoBehaviour
{
    public static RecapSceneManager instance = null;

    public SceneChange recapToMR;

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
