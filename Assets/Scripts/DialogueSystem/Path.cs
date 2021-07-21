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

    public bool checkAttitude;
    public CharacterName characterToCheck;
    public AttitudeComparison comparison;
    public int attitudeValueToCompare;

    public bool checkEvent;
    public GameManager.SaveableEvent eventToCheck;

    public bool changesAttitude;
    public CharacterName attitudeToChange;
    public int amountToAlter;

    public bool logsEvent;
    public GameManager.SaveableEvent eventToLog;

    public enum AttitudeComparison
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

            myScript.checkAttitude = GUILayout.Toggle(myScript.checkAttitude, "Attitude Check");
            if (myScript.checkAttitude && !myScript.checkEvent)
            {
                myScript.characterToCheck = (CharacterName)EditorGUILayout.EnumPopup("Character to Check", myScript.characterToCheck);
                myScript.comparison = (AttitudeComparison)EditorGUILayout.EnumPopup("Comparison", myScript.comparison);
                myScript.attitudeValueToCompare = EditorGUILayout.IntField("Amount", myScript.attitudeValueToCompare);
            }

            myScript.checkEvent = GUILayout.Toggle(myScript.checkEvent, "Event Check");
            if (myScript.checkEvent && !myScript.checkAttitude)
            {
                myScript.eventToCheck = (GameManager.SaveableEvent)EditorGUILayout.EnumPopup("Event to Check", myScript.eventToCheck);
            }

            myScript.changesAttitude = GUILayout.Toggle(myScript.changesAttitude, "Changes Attitude");
            if (myScript.changesAttitude)
            {
                myScript.attitudeToChange = (CharacterName)EditorGUILayout.EnumPopup("Attitude To Change", myScript.attitudeToChange);
                myScript.amountToAlter = EditorGUILayout.IntField("Amount", myScript.amountToAlter);
            }

            myScript.logsEvent = GUILayout.Toggle(myScript.logsEvent, "Logs Event");
            if (myScript.logsEvent)
            {
                myScript.eventToLog = (GameManager.SaveableEvent)EditorGUILayout.EnumPopup("Event to Log", myScript.eventToLog);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(myScript);
            }

        }


    }
    #endif

}





