using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Drag : MonoBehaviour
{
    
    public float cF = 1.0f;
    public float cFA = 1.0f;
    public float waterDensity = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        Utils utils = this.GetComponent<Utils>();
        Rigidbody rb = this.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;
        rb.AddForceAtPosition(-cF*utils.wettedSurface*waterDensity*Time.deltaTime*velocity.magnitude*velocity, utils.meanCenter);
        Debug.Log("Drag = " + -cF*utils.wettedSurface*waterDensity*Time.deltaTime*velocity.magnitude*velocity);
        Vector3 angularVelocity = rb.angularVelocity;
        rb.AddTorque(-cFA*utils.wettedSurface*waterDensity*Time.deltaTime*angularVelocity);
    }
}
