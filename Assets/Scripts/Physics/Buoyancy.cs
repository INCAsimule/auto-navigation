using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float g;
    public float waterDensity = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        g = Physics.gravity.magnitude;
        
        if (this.GetComponent<Utils>() == null)
        {
            Debug.LogError("No mesh found on hullMesh");
            return;
        }

    }

    void FixedUpdate()
    {
        Utils utils = this.GetComponent<Utils>();
        Rigidbody rb = this.GetComponent<Rigidbody>();
        
        if (float.IsNaN(utils.meanDepth)) {
            Debug.LogWarning("meanDepth is NaN");
            return;
        }
        Debug.DrawLine(utils.meanCenter, utils.meanCenter + utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up);
        Vector3 centerProj = new Vector3(utils.meanCenter.x, -utils.meanDepth, utils.meanCenter.z);
        Debug.DrawLine(centerProj, centerProj + Vector3.right);
        Debug.DrawLine(centerProj, centerProj + Vector3.back);
        
        rb.AddForceAtPosition(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up, utils.meanCenter);
        
        Debug.Log("Buoyancy = " + utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up);
        
        //rb.AddForce(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up);
    }

}
