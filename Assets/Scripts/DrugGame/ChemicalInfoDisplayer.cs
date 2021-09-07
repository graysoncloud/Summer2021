using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChemicalInfoDisplayer : MonoBehaviour
{

    private RectTransform background;
    private TextMeshProUGUI text;
    private RectTransform rectTransform;

    private Vector3 mousePosition;
    public Vector2 offset = new Vector2(10, 10);
    public float moveSpeed = 5f;
    private void Awake(){
        background = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        ChangeText("Test\nTest 2?\nTest 3");
        gameObject.SetActive(false);
    }

    public void ChangeText(string input){
        text.SetText(input);

        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 padding = new Vector2(1.5f, 1.5f);

        background.sizeDelta = textSize + padding;
    }


    private void Update(){
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed) + offset;
    }
}
