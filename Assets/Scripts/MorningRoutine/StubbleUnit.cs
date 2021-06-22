using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StubbleUnit : MonoBehaviour
{
    public Sprite[] stubbleSprites = new Sprite[5];
    public int length = 0;
    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stubbleSprites[length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLength(int len) { 
        length = len;
        spriteRenderer.sprite = stubbleSprites[length];
    }
}
