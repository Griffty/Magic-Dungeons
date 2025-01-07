
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestHandler), true)] // deprecated
public class QuestHandlerEditor : Editor
{
    private QuestHandler _handler;

    private void Awake()
    {
        _handler = (QuestHandler) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // if (GUILayout.Button("Add GiveQuest"))
        // {
        //     _handler
        // }
    }
}
