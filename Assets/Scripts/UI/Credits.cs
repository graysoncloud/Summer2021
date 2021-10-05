using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private Transform lastCredit;

    private bool ended = false;

    [SerializeField]
    private SceneChange toTitle;

    private void Start()
    {
        //.Log(lastCredit.position.y);
    }

    private void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0);
        if (lastCredit.position.y > 600 && !ended)
        {
            // switch scene stuff
            ended = true;
            PlayerPrefs.DeleteAll();
            GameManager.instance.StartSequence(toTitle);
            Debug.Log("Success");
        }
    }

}
