using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Path", menuName = "Path", order = 1)]
[System.Serializable]
public class Path : ScriptableObject
{
    [TextArea]
    public string choiceText;

    public bool checkStress;
    public ComparisonType stressComparison;
    public int stressCheckAmount;

    public bool checkAttitude;
    public CharacterName characterToCheck;
    public ComparisonType attitudeComparison;
    public int attitudeToCompare;

    public bool checkEvent;
    public GameManager.SaveableEvent eventToCheck;

    public bool checkNumAltContractsFinished;
    public int minimumRequired;

    public bool changesStress;
    public int stressChangeAmount;

    public bool changesAttitude;
    public CharacterName attitudeToChange;
    public int amountToAlterAttitude;

    public bool logsEvent;
    public GameManager.SaveableEvent eventToLog;

    public enum ComparisonType
    {
        greaterThanOrEqual,
        lessThanOrEqual
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Path))]
    public class ChoiceEditor : Editor
    {

        override public void OnInspectorGUI()
        {

            var myScript = target as Path;

            EditorGUILayout.LabelField("Choice Text:");
            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            myScript.choiceText = EditorGUILayout.TextArea(myScript.choiceText, style);

            // Checks
            myScript.checkAttitude = GUILayout.Toggle(myScript.checkAttitude, "Attitude Check");
            if (myScript.checkAttitude)
            {
                myScript.characterToCheck = (CharacterName)EditorGUILayout.EnumPopup("Character to Check", myScript.characterToCheck);
                myScript.attitudeComparison = (ComparisonType)EditorGUILayout.EnumPopup("Comparison", myScript.attitudeComparison);
                myScript.attitudeToCompare = EditorGUILayout.IntField("Amount", myScript.attitudeToCompare);
            }

            myScript.checkEvent = GUILayout.Toggle(myScript.checkEvent, "Event Check");
            if (myScript.checkEvent)
            {
                myScript.eventToCheck = (GameManager.SaveableEvent)EditorGUILayout.EnumPopup("Event to Check", myScript.eventToCheck);
            }

            myScript.checkStress = GUILayout.Toggle(myScript.checkStress, "Stress Check");
            if (myScript.checkStress)
            {
                myScript.stressComparison = (ComparisonType)EditorGUILayout.EnumPopup("Comparison", myScript.stressComparison);
                myScript.stressCheckAmount = EditorGUILayout.IntField("Amount", myScript.stressCheckAmount);
            }

            myScript.checkNumAltContractsFinished = GUILayout.Toggle(myScript.checkNumAltContractsFinished, "Alt Contracts Finished Check");
            if (myScript.checkNumAltContractsFinished)
            {
                myScript.minimumRequired = EditorGUILayout.IntField("Minimum Required", myScript.minimumRequired);
            }

            // Changes
            myScript.changesAttitude = GUILayout.Toggle(myScript.changesAttitude, "Changes Attitude");
            if (myScript.changesAttitude)
            {
                myScript.attitudeToChange = (CharacterName)EditorGUILayout.EnumPopup("Attitude To Change", myScript.attitudeToChange);
                myScript.amountToAlterAttitude = EditorGUILayout.IntField("Amount", myScript.amountToAlterAttitude);
            }

            myScript.logsEvent = GUILayout.Toggle(myScript.logsEvent, "Logs Event");
            if (myScript.logsEvent)
            {
                myScript.eventToLog = (GameManager.SaveableEvent)EditorGUILayout.EnumPopup("Event to Log", myScript.eventToLog);
            }

            myScript.changesStress = GUILayout.Toggle(myScript.changesStress, "Changes Stress");
            if (myScript.changesStress)
            {
                myScript.stressChangeAmount = EditorGUILayout.IntField("Amount", myScript.stressChangeAmount);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
            }

        }


    }
    #endif

}





