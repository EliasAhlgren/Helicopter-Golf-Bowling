using System;
using UnityEngine;
using UnityEngine.UI;

namespace HelicopterController
{
    /// <summary>
    /// Script for the helicopter main rotor which controls vertical movement, x and z rotation.
    /// </summary>
    public class RotorController : MonoBehaviour
    {
        public bool isPlayerControlled = true;
        
        public Transform physicalRotor;
        
        public string inputAxis;
        
        public float yThrust = 10;

        private Rigidbody _rigidbody;

        public AnimationCurve heightCurve;
        public AnimationCurve rotationCurve;
        
        public float yVelocity;
        public float maxYVelocity = 50;
        public float yDeceleration = 5;

        public float yDecelartionInFilight;
        
        public float xzRotationPower = 5;

        public Slider slider;
        
        // Start is called before the first frame update
        void Start()
        //dwdwdwde
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        
        public void Relase()
        {
            isPlayerControlled = !isPlayerControlled;
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            slider.value = yVelocity;
            
            Vector3 newRot = physicalRotor.transform.localEulerAngles;
            newRot.y += yVelocity * Time.deltaTime * 50;
            physicalRotor.transform.localEulerAngles = newRot;
            
            if (isPlayerControlled)
            {
                YMovement();
                RotationMovement();
            }
            else if (yVelocity > 0)
            {
                yVelocity -= Time.deltaTime * yDeceleration * yDecelartionInFilight;
            }
            _rigidbody.AddRelativeForce(0,yVelocity * heightCurve.Evaluate(transform.position.y), 0 ,ForceMode.Force);
            
        }

        void YMovement()
        {
            
            if ( Input.GetAxis(inputAxis) == 0 && yVelocity > 1)
            {
                
            }
            else if (yVelocity < maxYVelocity && yVelocity > -20)
            {
                yVelocity += Time.deltaTime * yThrust * Input.GetAxis(inputAxis);
            }
            else if (yVelocity > maxYVelocity)
            {
                yVelocity = maxYVelocity - 1;
            }
            else if (yVelocity < -20)
            {
                yVelocity = -19;
            }
            
        }

        private void RotationMovement()
        {
            
            
            Vector3 rotation = new Vector3(0, 0f, xzRotationPower * -Input.GetAxisRaw("Vertical") * rotationCurve.Evaluate(transform.rotation.eulerAngles.x));
            
            _rigidbody.AddRelativeForce(0,0,-Input.GetAxis("Horizontal"), ForceMode.Force);
            
            _rigidbody.AddRelativeTorque(rotation, ForceMode.Force);
            
        }

    }
}
