using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tiling3D))]
public class Tiling3DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Tiling3D data = (Tiling3D)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space(25);

        if (GUILayout.Button("Create building"))
            data.CreateBuilding();

        if (GUILayout.Button("Clear building"))
            data.ClearBuildingChildren();
    }
}
