using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using UnityEditor;
using UnityEngine;

public class EzBuoyancy : MonoBehaviour
{
    
    public Mesh hull;
    public float waterLevel = 0f;
    public float g = 9.81f;
    
    public float density_water = 1000f;
    
    public float total_pressure_force = 0f;
    
    Rigidbody rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }
    
    
    public static float ComputeTriangleArea(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        // Compute two edge vectors
        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;

        // Compute the cross product of the edge vectors
        Vector3 crossProduct = Vector3.Cross(edge1, edge2);

        // Compute the area (half the magnitude of the cross product)
        float area = crossProduct.magnitude * 0.5f;

        return area;
    }
    
    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(Vector3.down*rigidbody.mass*g);
        
        List<Vector3> vertices = new List<Vector3> {Vector3.zero, Vector3.zero, Vector3.zero};
        List<Vector3> normals = new List<Vector3> {Vector3.zero, Vector3.zero, Vector3.zero};
        Vector3 center = Vector3.zero;
        float surface = 0f;
        float pressure = 0f;
        Vector3 normal;
        
        total_pressure_force = 0f;
        
        for (int i = 0; i < hull.triangles.Length; i+=3) {
            vertices[0] = hull.vertices[hull.triangles[i]];
            vertices[1] = hull.vertices[hull.triangles[i+1]];
            vertices[2] = hull.vertices[hull.triangles[i+2]];
            
            center = vertices[0]/3f + vertices[1]/3f + vertices[2]/3f;
            center = transform.TransformPoint(center);
            
            if (center.y > waterLevel) {
                continue;
            }
            
            surface = ComputeTriangleArea(vertices[0], vertices[1], vertices[2]);
            
            normals[0] = hull.normals[hull.triangles[i]];
            normals[1] = hull.normals[hull.triangles[i+1]];
            normals[2] = hull.normals[hull.triangles[i+2]];
            
            normal = normals[0]/3f + normals[1]/3f + normals[2]/3f;
            normal = transform.TransformPoint(normal);
            pressure = (waterLevel - center.y) * density_water * g;
            
            if (Vector3.Dot(vertices[0]/3f + vertices[1]/3f + vertices[2]/3f, normals[0]/3f + normals[1]/3f + normals[2]/3f) > 0) {
                rigidbody.AddForceAtPosition(-pressure*surface*normal, center);
            } else {
                rigidbody.AddForceAtPosition(pressure*surface*normal, center);
            }
            
            total_pressure_force += pressure*surface*normal.y;
        }
        
        
        Debug.Log("" + total_pressure_force);
        
    }
}
