using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;

    public Room destination;

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

    void OnMouseDown() {
        FindObjectOfType<PlayerController>().SetRoom(destination);
    }

    void OnMouseExit() {
        spriteRenderer.sprite = closedSprite;
    }
}
