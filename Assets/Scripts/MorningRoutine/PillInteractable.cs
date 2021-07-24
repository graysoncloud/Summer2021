using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillInteractable : Interactable
{
    public bool dispensed = false;
    bool CRRunning = false;
    public bool taken = false;
    public float degreesToRotate = 30f;
    public float rotateStep = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    new void OnMouseDown() {
        if(dispensed) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            taken = true;
            CRRunning = false;
            dispensed = false;
            this.gameObject.SetActive(false);
            MorningRoutineManager.Instance.takenMedicationToday = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dispensed && CRRunning == false) {
            StartCoroutine("Wiggle");
            CRRunning = true;
        }
    }

    IEnumerator Wiggle() {
        bool dir = true;
        float angle = 0;
        while(!taken) {
            if(dir) { //clockwise
                angle += rotateStep;
                if(angle >= degreesToRotate) {
                    dir = false;
                }
            } else { //cc
                angle -= rotateStep;
                if(angle <= -degreesToRotate) {
                    dir = true;
                }
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Reset() {
        taken = false;
        CRRunning = false;
        dispensed = false;
    }
}
