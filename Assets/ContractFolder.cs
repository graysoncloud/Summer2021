using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractFolder : MonoBehaviour
{
    public Sprite unsolvedContractSprite;
    public Sprite solvedContractSprite;

    public bool pickedUp;
    public bool solved;

    private void Start()
    {
        pickedUp = true;
        solved = false;
        GetComponent<SpriteRenderer>().sprite = unsolvedContractSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            Vector3 mouseLocation = OfficeSceneCamera.instance.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseLocation.x, mouseLocation.y, 0);
        }
    }
}
