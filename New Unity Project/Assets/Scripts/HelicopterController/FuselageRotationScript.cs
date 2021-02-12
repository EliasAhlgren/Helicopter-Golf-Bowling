using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuselageRotationScript : MonoBehaviour
{
    public bool isPlayerControlled = true;
    
    private Rigidbody _rigidbody;

    public float rotSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void Relase()
    {
        isPlayerControlled = !isPlayerControlled;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isPlayerControlled)
        {
             _rigidbody.AddRelativeTorque(0, -Input.GetAxisRaw("YRotation") * rotSpeed,0,ForceMode.Force);
        }
       
    }
}
