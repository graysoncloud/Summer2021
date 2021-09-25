using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMinigame : Minigame
{
    public int numFlowers = 1;
    public GameObject flowerPotPrefab;
    List<GameObject> flowerPots = new List<GameObject>();
    public int[] flowerDecayList;

    Vector3 startPosition;
    public float spacing = 10f;

    public GameObject spawnPointGO;

    new void Start()
    {
        

        flowerDecayList = new int[numFlowers];

        startPosition = spawnPointGO.transform.position;

        for(int i = 0; i < numFlowers; i++) {
            Vector3 flowerPos = startPosition;
            flowerPos.x += (-(numFlowers / 2) * spacing) + (i * spacing);

            GameObject newFlower = Instantiate(flowerPotPrefab, flowerPos, Quaternion.identity);
            newFlower.transform.SetParent(this.transform);
            flowerPots.Add(newFlower);
            newFlower.GetComponent<FlowerPot>().UpdateFlower(flowerDecayList[i]);
        }

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void OnMouseDown() {
        base.OnMouseDown();
        
    }

    public void IncrementDay() {

        foreach(GameObject f in flowerPots) {
            f.GetComponent<FlowerPot>().Decay();
        }
    }
}
