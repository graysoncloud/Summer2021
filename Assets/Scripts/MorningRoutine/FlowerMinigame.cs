using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMinigame : Minigame
{
    public int numFlowers = 2;
    public GameObject flowerPotPrefab;
    List<GameObject> flowerPots = new List<GameObject>();
    public int[] flowerDecayList;

    Vector3 startPosition;
    public float spacing = 10f;

    new void Start()
    {
        

        flowerDecayList = new int[numFlowers];

        startPosition = gameObject.transform.position;

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
        for(int i = 0; i < numFlowers; i++) {
            flowerDecayList[i] = flowerPots[i].GetComponent<FlowerPot>().decay;
            flowerDecayList[i]++;
            flowerPots[i].GetComponent<FlowerPot>().UpdateFlower(flowerDecayList[i]);
        }
    }
}
