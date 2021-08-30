using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
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
    bool jittering = false;
    int jitterCooldown = 200;
    int jitterTimer = 0;
    int jitterDuration = 0;

    public bool hasAffector = false;




    // Start is called before the first frame update
    void Start()
    {
        activeCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultPosition = this.gameObject.transform.position;

        //gameObject.SetActive(persistent);
        if (persistent)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if(hasAffector) {
                transform.Find("Affector").gameObject.SetActive(false);
            }

    }

    // Update is called once per frame
    void Update()
    {
        if (active && mouseDown && draggable)
        {
            //Debug.Log("dragging " + interactableName);
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPos = activeCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, activeCam.nearClipPlane));
            screenPos.z = defaultPosition.z;

            int stress = PlayerPrefs.GetInt("Stress");
            this.gameObject.transform.position = screenPos;
            if (stress >= jitterThreshold && !jittering)
            {
                jittering = true;
                jitterTimer = (int)(jitterCooldown * ((100 + jitterThreshold - stress) / 100f));
                jitterDuration = Random.Range(10, stress);
            }
            if (jittering && jitterDuration > 0)
            {
                jitterDuration--;
                Jitter(screenPos);
            }
            else if (jittering)
            {
                jitterTimer--;
                if (jitterTimer <= 0)
                {
                    jittering = false;
                    jitterTimer = jitterCooldown;
                }
            }
        }


    }

    void Jitter(Vector3 startPos)
    {
        int stress = PlayerPrefs.GetInt("Stress");
        Vector3 jitterVec = Random.insideUnitSphere;
        float jitterMult = (PlayerPrefs.GetInt("Stress") / 100f) * maxJitter;
        jitterVec *= jitterMult;
        jitterVec.z = 0;
        startPos += jitterVec;
        this.gameObject.transform.position = startPos;

    }

    public void OnMouseDown()
    {
        if (GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive || SceneChangeManager.instance.IsFading())
        {
            return;
        }
        if (active && draggable)
        {
            if (!mouseDown)
            {
                defaultPosition = this.gameObject.transform.position;
            }
            //Debug.Log("Interactable " + interactableName + " clicked");
            Interact();
            mouseDown = true;
            if(hasAffector) {
                transform.Find("Affector").gameObject.SetActive(true);
            }
        }
    }

    public void OnMouseUp()
    {
        if (active && draggable)
        {
            mouseDown = false;
            this.gameObject.transform.position = defaultPosition;
            jittering = false;
            jitterTimer = 0;

            if(hasAffector) {
                transform.Find("Affector").gameObject.SetActive(false);
            }
        }
    }

    void Interact()
    {
    }

    public void SetInteractableActive(bool a)
    {
        active = a;
        GetComponent<BoxCollider2D>().enabled = a;

        if (!persistent)
        {
            gameObject.SetActive(a);
        }

        //Debug.Log(interactableName + " set to " + gameObject.active);
    }
}
