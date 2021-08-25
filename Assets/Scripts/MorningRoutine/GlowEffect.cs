using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    public bool shouldGlow = false;
    public bool hoverGlow = false;

    bool tutorialGlow = false;
    bool hovering = false;
    bool clicking = false;
    

    SpriteGlowEffect glowSettings;

    [Range(0, 5f)]
    public float maxGlow = 1f;
    [Range(0, 5f)]
    public float pulseSpeed = 0.1f;

    float glowTimer = 0f;
    public Color tutorialGlowColor;
    public Color hoverColor;

    int maxOutline;

    void Start()
    {
        if (shouldGlow)
        {
            tutorialGlow = true;
            glowSettings = GetComponent<SpriteGlowEffect>();
            maxOutline = glowSettings.OutlineWidth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && tutorialGlow && !hovering)
        {

            glowTimer += (pulseSpeed / 20);
            if (glowTimer >= 1f)
            {
                glowTimer = -1f;
            }

            float glowBrightness = Mathf.Abs(glowTimer) * maxGlow;
            glowSettings.GlowBrightness = glowBrightness;
            glowSettings.GlowColor = tutorialGlowColor;
            glowSettings.OutlineWidth = maxOutline;
        }
    }

    public void OnMouseDown()
    {
        glowSettings.GlowBrightness = 0;
        glowSettings.OutlineWidth = 0;
        if (tutorialGlow)
        {
            tutorialGlow = false;
        }

        clicking = true;
    }

    public void OnMouseDrag() {
        glowSettings.GlowBrightness = 0;
        glowSettings.OutlineWidth = 0;
    }

    public void OnMouseOver() {
        if(hoverGlow && !clicking) {
            hovering = true;
            glowSettings.OutlineWidth = maxOutline;
            glowSettings.GlowBrightness = maxGlow;
            glowSettings.GlowColor = hoverColor;
        }
    }

    public void OnMouseExit() { 
        if(hoverGlow) {
            hovering = false;
            glowSettings.GlowBrightness = 0;
            glowSettings.OutlineWidth = 0;
        }
        
    }

    public void OnMouseUp() {
        clicking = false;
    }
    
}
