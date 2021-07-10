using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blanket : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    bool dragging = false;
    bool bedMade = false;
    public GameObject madeBlanketGO;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bedMade) {
            spriteRenderer.enabled = false;
        } else {
            spriteRenderer.enabled = true;
        }
    }
    void OnMouseDown() {        
        spriteRenderer.sprite = sprites[1];
        dragging = true;
        Vector3 newScale = new Vector3(0.5f, 1, 1);
        GetComponent<Rigidbody2D>().transform.localScale = newScale;
    }

    void OnMouseUp() {
        spriteRenderer.sprite = sprites[0];
        dragging = false;
        Vector3 newScale = new Vector3(1, 1, 1);
        GetComponent<Rigidbody2D>().transform.localScale = newScale;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag.Equals("Bed")) {
            col.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //Debug.Log("collide with bed");
            Interactable i = GetComponent<Interactable>();
            i.OnMouseUp();
            i.SetInteractableActive(false);
            bedMade = true;
        }
    }

    public void Reset() {
        bedMade = false;
        madeBlanketGO.GetComponent<SpriteRenderer>().enabled = false;
    }
}
