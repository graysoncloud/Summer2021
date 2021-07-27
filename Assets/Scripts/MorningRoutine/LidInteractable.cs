using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidInteractable : MonoBehaviour
{
    bool clicked = false;
    bool active = false;
    bool open = false;

    Vector3 defaultPos;
    public Vector3 openPos;

    public GameObject pillGO;
    bool pillDispensed = false;
    Vector3 pillStartPos;
    public Vector3 pillDispensedPos;

    void Start() {
        defaultPos = transform.localPosition;
        openPos += defaultPos;
    }

    void Update() {

    }

    void OnMouseDown() {
        if(!ConversationManager.instance.inConversation && active) {
            //Debug.Log("lid clicked");
            IEnumerator coroutine;

            if(!open) { //if closed, open lid
                coroutine = MoveLid(defaultPos, openPos, 0.05f, true);
                StartCoroutine(coroutine);
                if(!pillDispensed) {
                    pillGO.SetActive(true);
                    pillDispensed = true;
                    pillStartPos = pillGO.transform.localPosition;
                    pillDispensedPos += pillStartPos;

                    IEnumerator pillCoroutine = MovePill(pillStartPos, pillDispensedPos, 0.05f);
                    StartCoroutine(pillCoroutine);
                }
            } else { // if open, close lid
                coroutine = MoveLid(openPos, defaultPos, 0.05f, false);
                StartCoroutine(coroutine);
            }
        }
    }

    public void SetActive(bool a) {
        //Debug.Log("lid set to " + a);
        active = a;
    }

    public void Reset() {
        //Debug.Log("reset");
        active = false;
        open = false;
        pillDispensed = false;
        pillGO.transform.localPosition = pillStartPos;
        transform.localPosition = defaultPos;
    }

    IEnumerator MoveLid(Vector3 posA, Vector3 posB, float step, bool newState) {

        for(float f = 0; f <= 1; f += step) {
            Vector3 pos = Vector3.Lerp(posA, posB, f);
            transform.localPosition = pos;
            yield return new WaitForSeconds(.01f);
        }
        transform.localPosition = posB;
        open = newState;
    } 

    IEnumerator MovePill(Vector3 posA, Vector3 posB, float step) {

        for(float f = 0; f <= 1; f += step) {
            Vector3 pos = Vector3.Lerp(posA, posB, f);
            pillGO.transform.localPosition = pos;
            yield return new WaitForSeconds(.01f);
        }
        pillGO.transform.localPosition = posB;
        pillGO.GetComponent<PillInteractable>().dispensed = true;
    } 


}