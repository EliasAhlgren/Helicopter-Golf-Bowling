using UnityEngine;

namespace HelicopterController
{
    /// <summary>
    /// Script for the helicopter main rotor which controls vertical movement, x and z rotation.
    /// </summary>
    public class RotorScript : MonoBehaviour
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

        public float xzRotationPower = 5;
        
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
            if (isPlayerControlled)
            {
                YMovement();
                RotationMovement();
            }
            
        }

        void YMovement()
        {
            Vector3 newRot = physicalRotor.transform.localEulerAngles;
            newRot.y += yVelocity * Time.deltaTime * 50;
            physicalRotor.transform.localEulerAngles = newRot;
            
            if (Input.GetAxis(inputAxis) == 0 && yVelocity > 1)
            {
                yVelocity -= Time.deltaTime * yDeceleration;
            }
            else if (yVelocity < maxYVelocity && yVelocity > -20)
            {
                yVelocity += Time.deltaTime * yThrust * Input.GetAxis(inputAxis);
            }
            else if (yVelocity < -20)
            {
                yVelocity = -19;
            }
            
            _rigidbody.AddRelativeForce(0,yVelocity * heightCurve.Evaluate(transform.position.y), 0 ,ForceMode.Force);    
        }

        private void RotationMovement()
        {
            
            
            Vector3 rotation = new Vector3(0, 0f, xzRotationPower * -Input.GetAxisRaw("Vertical") * rotationCurve.Evaluate(transform.rotation.eulerAngles.x));
            Debug.Log(xzRotationPower * -Input.GetAxisRaw("Vertical") * rotationCurve.Evaluate(transform.rotation.eulerAngles.x));            
            
            _rigidbody.AddRelativeForce(0,0,-Input.GetAxis("Horizontal"), ForceMode.Force);
            
            _rigidbody.AddRelativeTorque(rotation, ForceMode.Force);
            
        }

    }
}
