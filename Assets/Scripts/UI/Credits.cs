using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private Transform lastCredit;

    private bool ended = false;

    private void Start()
    {
        Debug.Log(lastCredit.position.y);
    }

    private void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0);
        if (lastCredit.position.y > 1080 && !ended)
        {
            // switch scene stuff
            Debug.Log("end");
            ended = true;
        }
    }

}
