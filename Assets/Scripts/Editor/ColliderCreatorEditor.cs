using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColliderCreator))]
class ColliderCreatorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        ColliderCreator cc = target as ColliderCreator;

        if (GUILayout.Button("create collider")) {
            cc.Start();
        }
    }
}