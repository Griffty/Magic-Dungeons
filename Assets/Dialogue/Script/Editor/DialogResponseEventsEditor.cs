
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DialogResponseEvents))]
public class DialogResponseEventsEditor : Editor
{
  public override void OnInspectorGUI()
  {
      DrawDefaultInspector();
      DialogResponseEvents responseEvents = (DialogResponseEvents)target;

      if (GUILayout.Button("Refresh"))
      {
          responseEvents.OnValidate();
      }
  }
}
