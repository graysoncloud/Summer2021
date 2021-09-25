using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamProp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Color colorIncrement = new Color(0f, 0f, 0f, .01f);
    private Vector3 rotationIncrement = new Vector3(0f, 0f, 1f);


    public void StartRotation()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);

        StartCoroutine("StandardPropCoroutine");
    }

    IEnumerator StandardPropCoroutine()
    {
        float totalTime = 0;
        
        while (totalTime < 5f)
        {
            if (totalTime < 2f)
            {
                spriteRenderer.color += colorIncrement * Time.deltaTime * 60;
            }

            else
            {
                spriteRenderer.color -= colorIncrement * Time.deltaTime * 60;
            }

            transform.eulerAngles += rotationIncrement * 60 * Time.deltaTime;

            totalTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        DreamPropController.instance.currentProps.Remove(this.gameObject);
        yield return new WaitForEndOfFrame();

    }

}
