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


    public SpriteGlowEffect glowSettings;

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

            if(!PlayerPrefs.HasKey(gameObject.name + "Glow")) {
                PlayerPrefs.SetInt(gameObject.name + "Glow", 1);
            }
            else if(PlayerPrefs.GetInt(gameObject.name + "Glow") == 0) {
                shouldGlow = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.instance.optionsMenuActive || GameManager.instance.sequenceActive || SceneChangeManager.instance.IsFading()))
        {
            if (gameObject.activeSelf && tutorialGlow && !hovering)
            {
                if (glowSettings == null)
                {
                    return;
                }
                glowTimer += pulseSpeed * Time.deltaTime;
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
        else
        {
            glowSettings.GlowBrightness = 0;
            glowSettings.OutlineWidth = 0;
        }
    }

    public void OnMouseDown()
    {
        if (glowSettings == null)
        {
            return;
        }
        glowSettings.GlowBrightness = 0;
        glowSettings.OutlineWidth = 0;
        if (tutorialGlow)
        {
            tutorialGlow = false;
        }

        clicking = true;
    }

    public void OnMouseDrag()
    {
        if (glowSettings == null)
        {
            return;
        }
        glowSettings.GlowBrightness = 0;
        glowSettings.OutlineWidth = 0;
    }

    public void OnMouseOver()
    {
        if (glowSettings == null)
        {
            return;
        }
        if (hoverGlow && !clicking)
        {
            hovering = true;
            glowSettings.OutlineWidth = maxOutline;
            glowSettings.GlowBrightness = maxGlow;
            glowSettings.GlowColor = hoverColor;
        }
    }

    public void OnMouseExit()
    {
        if (glowSettings == null)
        {
            return;
        }
        if (hoverGlow)
        {
            hovering = false;
            glowSettings.GlowBrightness = 0;
            glowSettings.OutlineWidth = 0;
        }

    }

    public void OnMouseUp()
    {
        clicking = false;
    }

}
