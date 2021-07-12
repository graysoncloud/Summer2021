using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveOfficeButton : MonoBehaviour
{
    public static LeaveOfficeButton instance = null;

    private void Start()
    {
        gameObject.SetActive(false);
    }

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
