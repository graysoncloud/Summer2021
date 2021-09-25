using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVReport : MonoBehaviour
{
    public Sprite[] newsSprites;
    SpriteRenderer spriteRenderer;
    int spriteIndex = 0;

    bool showingNews = false;

    [Range(0.0f, 2f)]
    public float minTime = 0.1f;
    [Range(0.0f, 2f)]
    public float maxTime = 0.6f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ShowStill() {
        float time = Random.Range(minTime, maxTime);

        spriteRenderer.sprite = newsSprites[spriteIndex];
        yield return new WaitForSeconds(time);
        if(showingNews) {
            spriteIndex++;
            if(spriteIndex >= newsSprites.Length) {
                spriteIndex = 0;
            }
            StartCoroutine("ShowStill");
        } else {
            yield return null;
        }
    }

    public void StartNews() {
        spriteRenderer.enabled = true;
        showingNews = true;
        StartCoroutine("ShowStill");
    }

    public void StopNews() {
        showingNews = false;
        spriteRenderer.enabled = false;
        StopAllCoroutines();
    }
}
