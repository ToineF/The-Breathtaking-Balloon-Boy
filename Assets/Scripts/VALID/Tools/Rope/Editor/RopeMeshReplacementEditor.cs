using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RopeMeshReplacement))]
public class RopeMeshReplacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RopeMeshReplacement data = (RopeMeshReplacement)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space(25);

        if (GUILayout.Button("Replace"))
            data.ReplaceAllChildren();
    }
}
