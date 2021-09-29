using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MigraineController : MonoBehaviour
{
    [Range(0, 5)]
    public float jitterAmount = 1;

    [Range(0, 5)]
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
        /*if(pulsing) {
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
        }*/

        if (Input.GetKeyDown(KeyCode.M))
        {
            //StartMigraine(new MigraineEvent());
        }
    }

    IEnumerator StartMigraineCoroutine(MigraineEvent mE)
    {
        

        pulseTime = pulseTime * ((float)pulseNum - 0.5f);

        DialogueUIManager.instance.SetUpForMigraine();

        pulsing = true;
        for (int i = 0; i < pulseNum; i++)
        {
            for (int j = 0; j < pulseTime; j++)
            {
                delta++;
                float w = Mathf.Clamp(Mathf.Sin((delta / pulseTime) * Mathf.PI * 2), 0.3f, 1);
                postProcessingVolume.weight = w;

                yield return new WaitForEndOfFrame();
            }
        }
        pulsing = false;

        postProcessingVolume.weight = 0;

        GameManager.instance.StartSequence(mE.nextEvent);
    }

    public void StartMigraine(MigraineEvent mE)
    {
        if (!pulsing)
        {
            SetMigraineParams(mE.pulseNum, mE.strength);
            delta = 0;
            postProcessingVolume.weight = 0;
            //GetComponent<Camera>().enabled = true;
            //mainCamera = c;
            //mainCamera.enabled = false;
            pulses = 0;
            postProcessingVolume.profile = migraineProfile;

            StartCoroutine(StartMigraineCoroutine(mE));
        }


    }

    public void SetMigraineParams(int _pulseNum, float _strength)
    {
        pulseNum = _pulseNum;
        strength = _strength;
    }
}
