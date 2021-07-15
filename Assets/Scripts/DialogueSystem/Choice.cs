using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Choice", menuName = "Choice", order = 1)]
[System.Serializable]
public class Choice : ScriptableObject
{
    [TextArea]
    public string choiceText;

    public bool attitudeCheck;
    public CharacterName characterToCheck;
    public AttitudeComparison comparison;
    public int amountToCompare;

    public bool eventCheck;
    public GameManager.SavedEvent eventToCheck;

    public bool changesAttitude;
    public CharacterName attitudeToChange;
    public int amountToChange;

    public bool logEvent;
    public GameManager.SavedEvent eventToLog;

    public enum AttitudeComparison
    {
        greaterThanOrEqual,
        lessThanOrEqual
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Choice))]
    public class ChoiceEditor : Editor
    {

        override public void OnInspectorGUI()
        {

            var myScript = target as Choice;

            EditorGUILayout.LabelField("Choice Text:");
            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            myScript.choiceText = EditorGUILayout.TextArea(myScript.choiceText, style);

            myScript.attitudeCheck = GUILayout.Toggle(myScript.attitudeCheck, "Attitude Check");
            if (myScript.attitudeCheck && !myScript.eventCheck)
            {
                myScript.characterToCheck = (CharacterName)EditorGUILayout.EnumPopup("Character to Check", myScript.characterToCheck);
                myScript.comparison = (AttitudeComparison)EditorGUILayout.EnumPopup("Comparison", myScript.comparison);
                myScript.amountToCompare = EditorGUILayout.IntField("Amount", myScript.amountToCompare);
            }

            myScript.eventCheck = GUILayout.Toggle(myScript.eventCheck, "Event Check");
            if (myScript.eventCheck && !myScript.attitudeCheck)
            {
                myScript.eventToCheck = (GameManager.SavedEvent)EditorGUILayout.EnumPopup("Event to Check", myScript.eventToCheck);
            }

            myScript.changesAttitude = GUILayout.Toggle(myScript.changesAttitude, "Changes Attitude");
            if (myScript.changesAttitude)
            {
                myScript.attitudeToChange = (CharacterName)EditorGUILayout.EnumPopup("Attitude To Change", myScript.attitudeToChange);
                myScript.amountToCompare = EditorGUILayout.IntField("Amount", myScript.amountToCompare);
            }

            myScript.logEvent = GUILayout.Toggle(myScript.logEvent, "Logs Event");
            if (myScript.logEvent)
            {
                myScript.eventToLog = (GameManager.SavedEvent)EditorGUILayout.EnumPopup("Event to Log", myScript.eventToLog);
            }

        }


    }
    #endif

}





