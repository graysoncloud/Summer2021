using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineLid : Interactable
{
    new void OnMouseDown() {
        Debug.Log("took medication!");
    }
}
