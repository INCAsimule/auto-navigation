using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    
    // Warning! This value might be updated after being used, at worst on frame of latency and inconsistency between different behaviours
    public float wettedSurface;
    public float wettedLength;
    public float meanDepth;
    public Vector3 meanCenter;
    
    public float waterHeight = 0f;
    
    public Mesh hull;
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("No mesh found on hullMesh");
            return;
        }

    }

    void Update()
    {
        //Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Mesh mesh = hull;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        // Ensure we have valid mesh and rigidbody references.
        if (mesh == null || rb == null)
            return;
        
        meanDepth = 0f;
        wettedSurface = 0f;
        meanCenter = Vector3.zero;
        
        // Iterate through triangles (each set of 3 indices makes a triangle).
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Get the indices for the current triangle.
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            // Transform local vertices to world coordinates.
            Vector3 p0 = transform.TransformPoint(vertices[index0]);
            Vector3 p1 = transform.TransformPoint(vertices[index1]);
            Vector3 p2 = transform.TransformPoint(vertices[index2]);

            // clamp the vertices y position if they are below the water surface
            p0.y = Math.Min(p0.y, waterHeight + 0.001f);
            p1.y = Math.Min(p1.y, waterHeight + 0.001f);
            p2.y = Math.Min(p2.y, waterHeight + 0.001f);
            
            // Calculate the center of the triangle.
            Vector3 center = (p0 + p1 + p2) / 3f;

            // Check if the triangle's center is below the water surface.
            if (center.y < waterHeight)
            {
                
                
                
                p0.y = 0;
                p1.y = 0;
                p2.y = 0;
                
                // Visualize the mesh underwater.
                Debug.DrawLine(p0, p1, Color.red);
                Debug.DrawLine(p1, p2, Color.red);
                Debug.DrawLine(p2, p0, Color.red);
                
                float depth = waterHeight - center.y;

                // Calculate triangle area (half the magnitude of the cross product).
                float area = Vector3.Cross(p1 - p0, p2 - p0).magnitude * 0.5f;
                
                meanDepth += depth*area;
                wettedSurface += area;
                meanCenter += center*area*depth;
            }
        }
        if (wettedSurface > 0) {
            meanCenter /= meanDepth;
            meanDepth /= wettedSurface;
        } else {
            meanCenter = Vector3.zero;
            meanDepth = 0;
        }
        
    }
}
