using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningRoutineController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneChangeManager.instance.currentScene != null) {
            if(SceneChangeManager.instance.currentScene.name != "MorningRoutineScene") {
            gameObject.SetActive(false);
        }
        else {
            if(!gameObject.activeSelf) {
                gameObject.SetActive(true);
            }
        }
        }
        
    }
}