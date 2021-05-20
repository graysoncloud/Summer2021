using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private Color defaultColor = new Color(1f, 1f, 1f, 1f);
    private Color highlightColor = new Color(1f, .63f, .63f, 1f);

    private void OnMouseOver()
    {
        this.GetComponent<SpriteRenderer>().color = highlightColor;
    }

    private void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.currentlyHeldChemical == null) return;

        Destroy(GameManager.instance.currentlyHeldChemical.gameObject);
        Debug.Log(GameManager.instance.currentlyHeldChemical);
    }
}
