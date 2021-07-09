using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveButton : MonoBehaviour
{

    private void OnMouseDown()
    {
        SceneChangeManager.instance.StartSceneChange(DrugManager.instance.officeToOfficeSceneChange);

    }

}
