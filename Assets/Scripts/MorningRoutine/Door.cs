using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver() {
        spriteRenderer.sprite = openSprite;
    }

    void OnMouseExit() {
        spriteRenderer.sprite = closedSprite;
    }
}
