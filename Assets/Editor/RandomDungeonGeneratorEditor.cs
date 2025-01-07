
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    private AbstractDungeonGenerator _generator;

    private void Awake()
    {
        _generator = (AbstractDungeonGenerator) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            double startTime = Time.realtimeSinceStartup;
            _generator.GenerateDungeon();
            double endTime = Time.realtimeSinceStartup;
            double dif = endTime - startTime;
            Debug.Log(dif+" ");
        }
    }
}
