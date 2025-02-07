using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{

    public float g = 9.81f;
    public float waterDensity = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Utils>() == null)
        {
            Debug.LogError("No mesh found on hullMesh");
            return;
        }

    }

    void Update()
    {
        Utils utils = this.GetComponent<Utils>();
        Rigidbody rb = this.GetComponent<Rigidbody>();
        
        Debug.Log("S = " + utils.wettedSurface + " | h = " + utils.meanDepth + " | B = " + utils.meanCenter);
        if (float.IsNaN(utils.meanDepth)) {
            Debug.LogWarning("meanDepth is NaN");
            return;
        }
        Debug.DrawLine(utils.meanCenter, utils.meanCenter + utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up*Time.deltaTime);
        Vector3 centerProj = new Vector3(utils.meanCenter.x, -utils.meanDepth, utils.meanCenter.z);
        Debug.DrawLine(centerProj, centerProj + Vector3.right);
        Debug.DrawLine(centerProj, centerProj + Vector3.back);
        
        rb.AddForceAtPosition(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up*Time.deltaTime, utils.meanCenter);
        
        //rb.AddForce(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up);
    }

}
