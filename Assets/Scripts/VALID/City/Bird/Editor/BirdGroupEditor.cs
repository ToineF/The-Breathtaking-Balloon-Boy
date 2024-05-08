using UnityEditor;
using UnityEngine;

namespace BlownAway.City
{
    [CustomEditor(typeof(BirdGroup))]
    public class BirdGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BirdGroup data = (BirdGroup)target;

            base.OnInspectorGUI();

            EditorGUILayout.Space(25);

            if (GUILayout.Button("Create Bird Group"))
                data.SpawnBirds();

            if (GUILayout.Button("Clear Bird Group"))
                data.Clear();
        }
    }
}