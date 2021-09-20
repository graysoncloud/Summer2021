using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCharacter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float time = 0f;

    // X should go from -16 to 5
    [SerializeField]
    private float xDir;

    // Y should go from -3.5 to -2.5
    [SerializeField]
    private float yDir;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        GetStartingDirections();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        float yMult = .5f * Mathf.Sin((Mathf.PI / 2) * time) - 3.5f;

        transform.position = new Vector3(transform.position.x + (Time.deltaTime * xDir), yMult, 0f);

        if (transform.position.x >= 5)
        {
            xDir = -1;
        }
        if (transform.position.x <= -16)
        {
            xDir = 1;
        }


    }

    private void GetStartingDirections()
    {
        if (transform.position.x < -5.5)
        {
            xDir = 1f;
        } 
        else
        {
            xDir = -1f;
        }

    }




}
