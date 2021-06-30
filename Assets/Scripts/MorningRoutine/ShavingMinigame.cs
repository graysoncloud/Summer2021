using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShavingMinigame : Minigame
{
    // Start is called before the first frame update
    public GameObject stubblePrefab, stubbleParent;
    List<StubbleUnit> stubbleUnits;
    public BoxCollider2D spawnBounds;


    Vector2 spawnMin, spawnMax;

    new void Start()
    {
        base.Start();
        Vector2 spawnPos = new Vector2(spawnBounds.transform.position.x, spawnBounds.transform.position.y);
        spawnMin = spawnPos - (spawnBounds.size / 2);
        spawnMax = spawnPos + (spawnBounds.size / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void OnMouseDown() { 
        Debug.Log("Minigame " + minigameName + " clicked");
        if(!isGameActive) {
            isGameActive = true;
            BeginGame();
        }
    }

    new void BeginGame() {
        base.BeginGame();

        //SpawnStubble();
        GetStubble();

        Debug.Log("shaving game begun");
    }

    void SpawnStubble() {
        int numStubbles = Random.Range(5, 10);
        for(int i = 0; i < numStubbles; i++) {
            float randX = Random.Range(spawnMin.x, spawnMax.x);
            float randY = Random.Range(spawnMin.y, spawnMax.y);
            Vector3 randomPos = new Vector3(randX, randY, 0);
            StubbleUnit newStubble = Instantiate(stubblePrefab, randomPos, Quaternion.identity).GetComponent<StubbleUnit>();
            newStubble.transform.SetParent(stubbleParent.transform);
            Debug.Log("stubble spawned");
        }
    }

    void GetStubble() {
        stubbleUnits = new List<StubbleUnit>(FindObjectsOfType<StubbleUnit>());
        Debug.Log(stubbleUnits.Count + " stubble units found");
    }

    public void IncrementDay() {
        foreach(StubbleUnit s in stubbleUnits) {
            s.UpdateLength(1);
        }
    }
}
