using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractStack : MonoBehaviour
{
    public static ContractStack instance = null;

    //public Sprite[] sprites;
    //public int currentSprite;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

}
