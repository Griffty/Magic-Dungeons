using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImageCreate), true)]
public class ImageCreatorEditor : Editor
{
    private ImageCreate _generator;
    private ImageGenerator _generator2;

    private void Awake()
    {
        _generator = (ImageCreate)target;
        _generator2 = FindObjectOfType<ImageGenerator>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Make Image"))
        {
            _generator.MakeImage(true);
        }
        
        if (GUILayout.Button("Save Figures To File"))
        {
            _generator.SaveSettings();
        }
    
        if (GUILayout.Button("Make Image Set"))
        {
            _generator2.MakeImageSet();
        }
    }
}