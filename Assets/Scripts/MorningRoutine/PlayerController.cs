using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float scrollSpeed = 0.5f;

    public GameObject[] rooms;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoom(int r) {
        for(int i = 0; i < rooms.Length; i++) {
            Vector3 pos = rooms[i].transform.position;
            if(i == r) {
                pos.x = 0;
            } else {
                pos.x = 25;
            }
            rooms[i].transform.position = pos;
        }
    }

    public void GoToWork() {
        GameObject officeScene = GameObject.Find("OfficeScene");
        GameObject MRScene = GameObject.Find("MorningRoutineScene");
        if(officeScene != null && MRScene != null) {
            officeScene.SetActive(true);
            MRScene.SetActive(false);
        }
    }
}
