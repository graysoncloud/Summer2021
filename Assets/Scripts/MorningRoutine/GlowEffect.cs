using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public bool shouldGlow = false;

    bool tutorialGlow = false;

    SpriteGlowEffect glowSettings;

    [Range(0, 5f)]
    public float maxGlow = 1f;
    [Range(0, 0.5f)]
    public float pulseSpeed = 0.1f;

    float glowTimer = 0f;

    void Start()
    {
        if (shouldGlow)
        {
            tutorialGlow = true;
            glowSettings = GetComponent<SpriteGlowEffect>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && tutorialGlow)
        {

            glowTimer += pulseSpeed;
            if (glowTimer >= 1f)
            {
                glowTimer = -1f;
            }


            float glowBrightness = Mathf.Abs(glowTimer) * maxGlow;
            glowSettings.GlowBrightness = glowBrightness;
        }
    }

    public void OnMouseDown()
    {
        if (tutorialGlow)
        {
            tutorialGlow = false;
            glowSettings.OutlineWidth = 0;
        }
    }
}
