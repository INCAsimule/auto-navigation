using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzBuoyancy : MonoBehaviour
{
    
    public Mesh hull;
    public float waterLevel = 0f;
    public float g = 9.81f;
    
    private Rigidbody rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(Vector3.down*rigidbody.mass*g);
        
        
        
    }
}
