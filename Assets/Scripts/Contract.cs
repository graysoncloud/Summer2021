using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

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

[CreateAssetMenu(fileName = "Contract", menuName = "Contract", order = 1)]

public class Contract : ScriptableObject
{
    // Need to check in with Harry to get specifics

    public static SceneAsset mainScene;

    public string companyName;
    public string description;

    // Volatility requirements (no contract would have a minimum volatility right?
    public bool usesMaxVolatility;
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
    public int desirableEffectMin;

    #if UNITY_EDITOR
    [CustomEditor(typeof(Contract))]
    public class ContractEditor : Editor
    {

        override public void OnInspectorGUI()
        {
            // Makes words wrap in text areas
            GUIStyle myCustomStyle = new GUIStyle(GUI.skin.GetStyle("TextArea"))
            {
                wordWrap = true
            };

            var myScript = target as Contract;

            myScript.companyName = EditorGUILayout.TextField("Company Name", myScript.companyName);
            EditorGUILayout.LabelField("Description:");

            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            myScript.description = EditorGUILayout.TextArea(myScript.description, style);

            // Max Volatility
            myScript.usesMaxVolatility = GUILayout.Toggle(myScript.usesMaxVolatility, "Uses Max Volatility");
            if (myScript.usesMaxVolatility)
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
                myScript.undesirableEffect = (EffectType)EditorGUILayout.EnumPopup("Effect Type", myScript.undesirableEffect);
                myScript.undesirableEffectMax = EditorGUILayout.IntField("Max", myScript.undesirableEffectMax);
            }

            // Desirable Effect
            myScript.usesDesirableEffect = GUILayout.Toggle(myScript.usesDesirableEffect, "Uses Desirable Effect");
            if (myScript.usesDesirableEffect)
            {
                myScript.desirableEffect = (EffectType)EditorGUILayout.EnumPopup("Effect Type", myScript.desirableEffect);
                myScript.desirableEffectMin = EditorGUILayout.IntField("Min", myScript.desirableEffectMin);
            }

            // Could add multiple desirable / undesirable effects
            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
            }

            EditorStyles.textArea.wordWrap = true;

        }


    }
    #endif

}