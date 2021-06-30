using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : Interactable
{
    // Start is called before the first frame update
    [Range(0, 4)]
    [SerializeField]
    public int decay = 0;
    public int waterLevel = 0;
    int maxWaterLevel;
    int maxDecay;
    public Sprite[] flowerSprites;
    public GameObject[] moistureBars;
    public SpriteRenderer spriteRenderer;
    public GameObject waterEffectGO;

    bool hasBeenWateredToday = false;
    
    void Start() {
        maxDecay = flowerSprites.Length - 1;
        maxWaterLevel = moistureBars.Length;
        waterEffectGO = GameObject.FindGameObjectWithTag("WaterEffect");
        waterEffectGO.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = flowerSprites[decay];
        for(int i = 0; i < maxWaterLevel; i++) {
            
            if(waterLevel > i) {
                moistureBars[i].SetActive(true);
            } else {
                moistureBars[i].SetActive(false);
            }
            
        }
    }

    public void Reset() {

    }

    public void UpdateFlower(int d) { 
        /*decay = d;
        hasBeenWateredToday = false;
        if(decay > maxDecay) { 
            decay = maxDecay;
        }*/

        if(waterLevel > 0) {
            waterLevel--;
        } else {
            decay++;
            hasBeenWateredToday = false;
            if(decay > maxDecay) { 
                decay = maxDecay;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("collide with " + col.name);
        if(col.gameObject.tag.Equals("WateringCan")) {
            Debug.Log("water");
            if(!hasBeenWateredToday) {
                
                if(decay > 0) {
                    decay -= 1;
                    hasBeenWateredToday = true;                   
                }
                
            }

            if(waterLevel < maxWaterLevel) {
                waterLevel++;
                StartCoroutine("ShowWaterEffect");
            }
        }
    }

    IEnumerator ShowWaterEffect() {
        waterEffectGO.transform.position = spriteRenderer.transform.position;
        waterEffectGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        waterEffectGO.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        waterEffectGO.SetActive(false);
    }
}
