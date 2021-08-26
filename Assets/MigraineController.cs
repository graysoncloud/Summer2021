using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MigraineController : MonoBehaviour
{
    [Range(0,5)]
    public float jitterAmount = 1;

    [Range(0,5)]
    public float strength = 1;

    [Range(0, 1000)]
    public int pulseTime = 100;

    public bool active = false;
    int pulse = 0;
    bool increasing = true;

    Camera activeCamera;

    Vector3 initialPos;

    PostProcessVolume postProcessingVolume;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        postProcessingVolume = GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active) {
            if(increasing) {
                postProcessingVolume.weight = Mathf.Lerp(0, 100, (pulse / pulseTime));
                pulse++;
                Vector3 rand = Random.insideUnitSphere;
                rand.z = 0;
                rand *= jitterAmount;
                transform.localPosition = initialPos + rand;
                if(pulse >= pulseTime) {
                    increasing = false;
                    transform.localPosition = initialPos;
                }
            } else {
                pulse--;
                if(pulse <= 0) {
                    increasing = true;
                }
            }
            
        }
    }
}
