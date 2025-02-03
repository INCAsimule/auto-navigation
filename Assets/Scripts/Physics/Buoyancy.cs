using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    public float waterHeight = 0f;
    public float gravity = 9.81f;
    public float buoyancyFactor = 0.5f;
    
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
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        // Ensure we have valid mesh and rigidbody references.
        if (mesh == null || rb == null)
            return;

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

            // Visualize the mesh underwater.
            Debug.DrawLine(p0, p1, Color.red);
            Debug.DrawLine(p1, p2, Color.red);
            Debug.DrawLine(p2, p0, Color.red);


            // Check if the triangle's center is below the water surface.
            if (center.y < waterHeight)
            {
                float depth = waterHeight - center.y;
                // Calculate the average normal of the triangle.
                Vector3 normal0 = transform.TransformDirection(mesh.normals[index0]);
                Vector3 normal1 = transform.TransformDirection(mesh.normals[index1]);
                Vector3 normal2 = transform.TransformDirection(mesh.normals[index2]);

                Vector3 normal = (normal0 + normal1 + normal2) / 3f;
                normal.Normalize();

                // Calculate triangle area (half the magnitude of the cross product).
                float area = Vector3.Cross(p1 - p0, p2 - p0).magnitude * 0.5f;

                // Compute buoyancy force (force proportional to pressure, area, and a tuning factor).
                float buoyancyForce = depth * area * gravity * buoyancyFactor;

                // Apply the upward force at the triangle's center.
                rb.AddForceAtPosition(-normal * buoyancyForce, center);
                // Draw a debug line to visualize the applied force.
                Debug.DrawRay(center, -normal * buoyancyForce, Color.blue);
                Debug.Log("Applied buoyancy: " + buoyancyForce + " at position: " + center);
            }
        }
    }

}
