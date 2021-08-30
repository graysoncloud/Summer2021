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

    public float pulseTime = 1;

    public bool active = false;
    float delta = 0;
    bool increasing = true;

    Camera activeCamera;

    Vector3 initialPos;

    public bool pulsing = false;

    PostProcessVolume postProcessingVolume;
    Camera mainCamera;

    [Range(0, 10)]
    public int pulseNum = 1;
    int pulses = 0;

    public PostProcessProfile migraineProfile;
    public PostProcessProfile normalProfile;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        postProcessingVolume = GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pulsing) {
            delta += Time.deltaTime;

            float w = Mathf.Clamp(Mathf.Sin((delta / pulseTime) * Mathf.PI * 2), 0.3f, 1);

            postProcessingVolume.weight = w;

            if(delta >= pulseTime) {
                pulses++;
                delta = 0;
                if(pulses >= pulseNum) {
                    pulsing = false;
                    //mainCamera.enabled = true;
                    //GetComponent<Camera>().enabled = false;
                    postProcessingVolume.profile = normalProfile;
                    postProcessingVolume.weight = 1;
                }
            }
        }
    }

    public void SetMigraineActive(bool t, Camera c) {
        


        pulsing = t;
        if(t) {
            delta = 0;
            postProcessingVolume.weight = 0;
            //GetComponent<Camera>().enabled = true;
            //mainCamera = c;
            //mainCamera.enabled = false;
            pulses = 0;
            postProcessingVolume.profile = migraineProfile;
        }
    }
}
