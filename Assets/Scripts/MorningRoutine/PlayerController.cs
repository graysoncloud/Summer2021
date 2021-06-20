using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float scrollSpeed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ScrollLeft() {
        transform.Translate(new Vector3(-scrollSpeed, 0, 0));
    }

    public void ScrollRight() {
        transform.Translate(new Vector3(scrollSpeed, 0, 0));
    }
}
