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
        
        rb.AddForceAtPosition(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up*Time.deltaTime, utils.meanCenter);
        //rb.AddForce(utils.wettedSurface*utils.meanDepth*g*waterDensity*Vector3.up);
    }

}
