using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionPaper : MonoBehaviour
{
    public bool pickedUp;

    private void Start()
    {
        pickedUp = true;
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

    public void RemoveThisSP()
    {
        Destroy(this);
    }
}
