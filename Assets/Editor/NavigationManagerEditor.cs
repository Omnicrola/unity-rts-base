using DefaultNamespace.Navigation;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(NavigationManager))]
    public class NavigationManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var navigationManager = (NavigationManager) target;

            GUILayout.Label("");
            GUILayout.Label("Elapsed time: " + navigationManager.ElapsedTime + "ms");
            if (GUILayout.Button("Find Path"))
            {
                navigationManager.FindPath();
            }
        }
    }
}