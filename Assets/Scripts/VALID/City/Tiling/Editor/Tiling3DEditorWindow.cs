using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Tiling3DEditorWindow : EditorWindow
{
    [MenuItem("Window/Tiling3D")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Tiling3DEditorWindow));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Regenerate all building"))
        {
            Tiling3D[] tilings = (Tiling3D[])FindObjectsOfType(typeof(Tiling3D));
            foreach (Tiling3D t in tilings)
            {
                t.CreateBuilding();
            }
        }
    }
}
