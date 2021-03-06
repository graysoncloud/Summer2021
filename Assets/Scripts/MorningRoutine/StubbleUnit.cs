using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StubbleState {
    UNSHAVED,
    CREAM,
    SHAVED,
    CUT
}

public class StubbleUnit : MonoBehaviour
{
    public Sprite[] stubbleSprites = new Sprite[5];
    public Sprite shaveCreamSprite;
    
    public Sprite cutSprite;

    public int length = 0;
    SpriteRenderer spriteRenderer;

    public float cutChance = 0.1f;
    public float healChance = 0.5f;

    StubbleState stubbleState = StubbleState.UNSHAVED;


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

        if(stubbleState == StubbleState.CUT) {
            float rand = Random.Range(0f, 1f);
            if(rand <= healChance) {
                return;
            }
        }

        length += len;
        if(length >= stubbleSprites.Length) {
            length = stubbleSprites.Length - 1;
        }
        spriteRenderer.sprite = stubbleSprites[length];
        stubbleState = StubbleState.UNSHAVED;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag.Equals("ShavingCream") && stubbleState == StubbleState.UNSHAVED) {
            //Debug.Log("creamed up");
            stubbleState = StubbleState.CREAM;
        }

        if(col.gameObject.tag.Equals("Razor")) {
            //Debug.Log("razor");
            
            if(stubbleState == StubbleState.CREAM) {
                stubbleState = StubbleState.SHAVED;
                length = 0;
            } else if(stubbleState == StubbleState.UNSHAVED) {
                float rand = Random.Range(0f, 1f);
                if(rand <= cutChance) {
                    stubbleState = StubbleState.CUT;
                } else {
                    stubbleState = StubbleState.SHAVED;
                }

                length = 0;
            }
        }

        UpdateSprite();
    }

    void UpdateSprite() {
        switch(stubbleState) {
            case StubbleState.CREAM:
                spriteRenderer.sprite = shaveCreamSprite;
                break;
            case StubbleState.SHAVED:
                spriteRenderer.sprite = stubbleSprites[0];
                break;
            case StubbleState.CUT:
                spriteRenderer.sprite = cutSprite;
                break;
            default:
                break;
        }
    }
}
