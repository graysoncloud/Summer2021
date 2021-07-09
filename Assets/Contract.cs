using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Contract : MonoBehaviour
{
    // Need to check in with Harry to get specifics

    // Volatility requirements (no contract would have a minimum volatility right?
    public bool usesVolatility;
    public int volatilityMax;

    // Price requirements
    public bool usesMaxPrice;
    public int maxPrice;

    public bool usesMinPrice;
    public int minPrice;

    // Could expand to having multiple side effects

    // Undesirable side effect
    public bool usesUndesirableEffect;
    public EffectType undesirableEffect;
    public int undesirableEffectMax;

    // Desirable side effect
    public bool usesDesirableEffect;
    public EffectType desirableEffect;
    public int desirableEffectMax;


    public enum EffectType
    {
        Analgesic,
        Antidepressant,
        Ecstasis,
        FetalExposure,
        Headache,
        Immunosuppressant,
        Nauseatic,
        Stimulant,

    }


    [CustomEditor(typeof(Contract))]
    public class MyScriptEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var myScript = target as Contract;

            // Max Volatility
            myScript.usesVolatility = GUILayout.Toggle(myScript.usesVolatility, "Uses Max Volatility");
            if (myScript.usesVolatility)
            {
                myScript.volatilityMax = EditorGUILayout.IntField("Max Volatility", myScript.volatilityMax);
            }

            // Min Price
            myScript.usesMinPrice = GUILayout.Toggle(myScript.usesMinPrice, "Uses Min Price");
            if (myScript.usesMinPrice)
            {
                myScript.minPrice = EditorGUILayout.IntField("Min Price", myScript.minPrice);
            }

            // Max Price
            myScript.usesMaxPrice = GUILayout.Toggle(myScript.usesMaxPrice, "Uses Max Price");
            if (myScript.usesMaxPrice)
            {
                myScript.maxPrice = EditorGUILayout.IntField("Max Price", myScript.maxPrice);
            }

            // Undesirable Effect
            myScript.usesUndesirableEffect = GUILayout.Toggle(myScript.usesUndesirableEffect, "Uses Undesirable Effect");
            if (myScript.usesUndesirableEffect)
            {
                myScript.undesirableEffect = (EffectType)EditorGUILayout.EnumFlagsField("Effect Type", myScript.undesirableEffect);
                myScript.undesirableEffectMax = EditorGUILayout.IntField("Max", myScript.maxPrice);
            }

            // Desirable Effect
            myScript.usesDesirableEffect = GUILayout.Toggle(myScript.usesDesirableEffect, "Uses Desirable Effect");
            if (myScript.usesDesirableEffect)
            {
                myScript.desirableEffect = (EffectType)EditorGUILayout.EnumFlagsField("Effect Type", myScript.desirableEffect);
                myScript.desirableEffectMax = EditorGUILayout.IntField("Min", myScript.maxPrice);
            }

            // Could add multiple desirable / undesirable effects

        }
    }

}
