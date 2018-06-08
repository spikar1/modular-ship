using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class ColliderCreator : MonoBehaviour {
    public Mesh collisionMesh;
    private int currentPathIndex = 0;
    private PolygonCollider2D polygonCollider;
    private List<Edge> edges = new List<Edge>();
    private List<Vector2> points = new List<Vector2>();
    private Vector3[] vertices;

    public void Start() {
        currentPathIndex = 0;
        // Get the polygon collider (create one if necessary)
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null) {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            polygonCollider.pathCount = 0;
        }
        // Get the mesh's vertices for use later
        vertices = collisionMesh.vertices;//GetComponent<MeshFilter>().sharedMesh.vertices;

        // Get all edges from triangles
        int[] triangles = collisionMesh.triangles;// GetComponent<MeshFilter>().sharedMesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3) {
            edges.Add(new Edge(triangles[i], triangles[i + 1]));
            edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
            edges.Add(new Edge(triangles[i + 2], triangles[i]));
        }

        // Find duplicate edges
        List<Edge> edgesToRemove = new List<Edge>();
        foreach (Edge edge1 in edges) {
            foreach (Edge edge2 in edges) {
                if (edge1 != edge2) {
                    if (edge1.vert1 == edge2.vert1 && edge1.vert2 == edge2.vert2 || edge1.vert1 == edge2.vert2 && edge1.vert2 == edge2.vert1) {
                        edgesToRemove.Add(edge1);
                    }
                }
            }
        }

        // Remove duplicate edges (leaving only perimeter edges)
        foreach (Edge edge in edgesToRemove) {
            edges.Remove(edge);
        }

        // Start edge trace
        EdgeTrace(edges[0]);
        return;
    }



    void EdgeTrace(Edge edge) {
        // Add this edge's vert1 coords to the point list
        points.Add(vertices[edge.vert1]);

        // Store this edge's vert2
        int vert2 = edge.vert2;

        // Remove this edge
        edges.Remove(edge);

        // Find next edge that contains vert2
        foreach (Edge nextEdge in edges) {
            if (nextEdge.vert1 == vert2) {
                EdgeTrace(nextEdge);
                return;
            }
        }
        if (points.ToArray().Length != 1) {
            // No next edge found, create a path based on these points
            polygonCollider.pathCount = currentPathIndex + 1;
            polygonCollider.SetPath(currentPathIndex, points.ToArray());



            // Empty path
            points.Clear();

            // Increment path index
            currentPathIndex++;
        }
        // Start next edge trace if there are edges left
        if (edges.Count > 0) {
            EdgeTrace(edges[0]);
        }
    }
}

class Edge {
    public int vert1;
    public int vert2;

    public Edge(int Vert1, int Vert2) {
        vert1 = Vert1;
        vert2 = Vert2;
    }
}
[CustomEditor(typeof(ColliderCreator))]
class ColliderCreatorEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        ColliderCreator cc = target as ColliderCreator;

        if(GUILayout.Button("create collider")) {
            cc.Start();
        }
    }
}