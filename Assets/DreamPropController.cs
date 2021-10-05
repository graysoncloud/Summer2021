using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamPropController : MonoBehaviour
{
    public static DreamPropController instance;

    // relies on sprite-based prefabs
    public GameObject[] generatables;

    public List<GameObject> currentProps;

    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

        // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("PropCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginPropCoroutine()
    {
        StartCoroutine("PropCoroutine");
    }

    IEnumerator PropCoroutine ()
    {
        yield return new WaitForSeconds(2f);

        int lastSpriteIndex = 0;

        while (true)
        {
            int rIndex = Random.Range(0, generatables.Length);

            if (rIndex == lastSpriteIndex)
            {
                if (rIndex > 0)
                {
                    rIndex--;
                } else
                {
                    rIndex++;
                }
            }

            lastSpriteIndex = rIndex;

            int rx = Random.Range(xMin, xMax + 1);
            int ry = Random.Range(yMin, yMax + 1);
            Debug.Log(xMin + ", " + xMax);

            GameObject toAdd = Instantiate(generatables[rIndex], this.transform);
            toAdd.GetComponent<DreamProp>().StartRotation();
            toAdd.transform.position = new Vector3(rx, ry, 0);
            currentProps.Add(toAdd);

            float rFloat = Random.Range(3.5f, 5f);
            yield return new WaitForSeconds(rFloat);
        }
    }

    public void StopPropCoroutine()
    {
        foreach (GameObject prop in currentProps)
        {
            Destroy(prop);
        }
        StopAllCoroutines();
    }

    
}
