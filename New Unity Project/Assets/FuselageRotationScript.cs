using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuselageRotationScript : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float rotSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.AddRelativeTorque(0, -Input.GetAxisRaw("YRotation") * rotSpeed,0,ForceMode.Force);
    }
}
