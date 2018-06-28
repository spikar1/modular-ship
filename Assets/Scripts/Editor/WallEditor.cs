using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Wall))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("SMASH SMASH SMASH"))
            ((Wall) target).Damage(10f);
        if (GUILayout.Button("Update wall orientations"))
            UpdateWallOrientations();
    }

    private void UpdateWallOrientations()
    {
        var walls = FindObjectsOfType<Wall>();
        foreach (var wall in walls)
        {
            var node = wall.GetComponent<RoomNode>();
            if (node == null)
            {
                Debug.LogWarning("Wall without node!");
                continue;
            }
            var so = new SerializedObject(wall);
            so.FindProperty(nameof(wall.orientation)).enumValueIndex = (int) node.wallOrientation;
            so.ApplyModifiedProperties();
        }
    }
}