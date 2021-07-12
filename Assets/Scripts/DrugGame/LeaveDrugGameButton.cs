using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveDrugGameButton : MonoBehaviour
{

    private void OnMouseDown()
    {
        SceneChangeManager.instance.StartSceneChange(DrugManager.instance.drugToOfficeSceneChange);

    }

}
