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
        foreach (Day.Sequence dialogueEvent in GameManager.instance.currentDay.sequences)
        {
            if (dialogueEvent.trigger.ToString() == "leavingHome")
            {
                GameManager.instance.StartSequence(dialogueEvent.initialEvent);
                return;
            }

        }

        SceneChangeManager.instance.StartSceneChange(MorningRoutineManager.Instance.mrToOffice);

    }
}
