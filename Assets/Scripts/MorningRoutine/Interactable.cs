using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableName;
    public bool draggable;
    public bool persistent = false;
    private bool active = false;
    private bool mouseDown = false;
    private Vector3 defaultPosition;

    public Camera activeCam;


    // Start is called before the first frame update
    void Start()
    {
        activeCam = FindObjectOfType<Camera>();
        defaultPosition = this.gameObject.transform.position;

        //gameObject.SetActive(persistent);
        if(persistent) { 
            GetComponent<BoxCollider2D>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active && mouseDown && draggable) { 
            //Debug.Log("dragging " + interactableName);
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPos = activeCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, activeCam.nearClipPlane));
            screenPos.z = defaultPosition.z;
            this.gameObject.transform.position = screenPos;
            
        }
    }

    void OnMouseDown() {
        if(active) {
            Debug.Log("Interactable " + interactableName + " clicked");
            Interact();
            mouseDown = true;
        }
    }

    void OnMouseUp() {
        if(active) { 
            mouseDown = false;
            this.gameObject.transform.position = defaultPosition;
        }
    }

    void Interact() {
    }

    public void SetInteractableActive(bool a) { 
        active = a;
        GetComponent<BoxCollider2D>().enabled = a;

        if(!persistent) { 
            gameObject.SetActive(a);
        }
        
        //Debug.Log(interactableName + " set to " + gameObject.active);
    }
}
