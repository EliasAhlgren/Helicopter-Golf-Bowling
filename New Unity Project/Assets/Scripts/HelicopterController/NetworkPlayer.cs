using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetworkPlayer : MonoBehaviour
{
     
    public float yMovement;
    public float xRotation;
    public float yRotation;
    public float zRotation;
    [SerializeField] private string yMoveControl;
    [SerializeField] private string xRotationControl;
    [SerializeField] private string yRotationControl;
    [SerializeField] private string zRotationControl;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        yMovement = Input.GetAxisRaw(yMoveControl);
        xRotation = Input.GetAxisRaw(xRotationControl);
        yRotation = Input.GetAxisRaw(yRotationControl);
        zRotation = Input.GetAxisRaw(zRotationControl);
    }
}
