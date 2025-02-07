using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }

    public Boolean time = false;

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        
        if (time == true) {
            rb.AddForce(rb.mass*9.81f*Time.fixedDeltaTime*Vector3.up, ForceMode.Acceleration);
        } else {
            //rb.AddForce(-Physics.gravity * rb.mass);
            rb.AddForce(-((Physics.gravity * rb.mass)/Time.fixedDeltaTime)*Time.deltaTime);
        }
    }
}
