using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeComputer : MonoBehaviour
{
    public static OfficeComputer instance = null;
    //public bool mouseClicked;

    //public SceneChange computerToDrugGameTransition;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    //private void OnMouseDown()
    //{
    //    mouseClicked = true;
    //    SceneChangeManager.instance.StartSceneChange(computerToDrugGameTransition);

    //}

    //private void OnMouseUp()
    //{
    //    Vector3 pointOfClick = SceneChangeManager.instance.currentScene.transform.Find("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    //    Vector3 toEvaluate = new Vector3(pointOfClick.x, pointOfClick.y, transform.position.z);

    //    if (GetComponent<BoxCollider2D>().bounds.Contains(pointOfClick) && mouseClicked) {
    //        SceneChangeManager.instance.StartSceneChange(computerToDrugGameTransition);
    //    }

    //    mouseClicked = false;
    //}


}
