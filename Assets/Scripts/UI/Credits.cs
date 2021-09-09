using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    private void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0);
    }

}
