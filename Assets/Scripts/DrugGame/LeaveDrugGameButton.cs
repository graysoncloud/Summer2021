using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveDrugGameButton : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (GameManager.instance.optionsMenuActive)
            return;
        if (TutorialManager.instance.activeTutorial == null)
            SceneChangeManager.instance.StartSceneChange(DrugManager.instance.drugToOfficeSceneChange);

    }

}
