using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Thruster))]
public class ThrusterEditor : Editor {

    private Thruster thruster;

    private void OnEnable() {
        thruster = (Thruster) target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Center on axis")) {
            Undo.RecordObject(thruster.transform, "Centering thruster");
            var rb = thruster.GetComponentInParent<Rigidbody2D>();
            var center = rb.centerOfMass;
            Debug.Log(center);
            var newThrustPos = thruster.transform.localPosition;

            var thrustDir = thruster.GetThrustDirection();
            if (thrustDir == Direction.N || thrustDir == Direction.S) {
                newThrustPos.x = center.x;
            }
            else if(thrustDir == Direction.E || thrustDir == Direction.W) {
                newThrustPos.y = center.y;
            }

            thruster.transform.localPosition = newThrustPos;
        }
    }

}
