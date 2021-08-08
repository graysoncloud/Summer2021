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

    public AudioClip soundEffect;

    public int jitterThreshold = 20;
    public float maxJitter = 0.1f;


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
            

            if(PlayerPrefs.GetInt("Stress") >= jitterThreshold) {
                /*Vector3 jitterVec = Random.insideUnitSphere;
                float jitterMult = (PlayerPrefs.GetInt("Stress") / 100f) * maxJitter;
                jitterVec *= jitterMult;
                jitterVec.z = 0;
                screenPos += jitterVec;*/

                StartCoroutine("Jitter", screenPos);
            }

            this.gameObject.transform.position = screenPos;
        }
    }

    IEnumerator Jitter(Vector3 startPos) {
        int stress = PlayerPrefs.GetInt("Stress");
        int duration = Random.Range(10 * stress, 25 * stress);
        for(int i = 0; i < duration; i++) {
            Vector3 jitterVec = Random.insideUnitSphere;
            float jitterMult = (PlayerPrefs.GetInt("Stress") / 100f) * maxJitter;
            jitterVec *= jitterMult;
            jitterVec.z = 0;
            startPos += jitterVec;
            this.gameObject.transform.position = startPos;

            yield return new WaitForEndOfFrame();
            
        }
    }

    public void OnMouseDown() {
        if(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive) {
            return;
        }
        if(active && draggable) {
            if(!mouseDown) {
                defaultPosition = this.gameObject.transform.position;
            }
            Debug.Log("Interactable " + interactableName + " clicked");
            Interact();
            mouseDown = true;
        }
    }

    public void OnMouseUp() {
        if(active && draggable) { 
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
