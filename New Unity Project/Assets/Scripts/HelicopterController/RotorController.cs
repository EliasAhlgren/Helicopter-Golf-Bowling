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
        public GameManager gameManager;
        
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
        Slider slider;
        [HideInInspector] public NetworkPlayer networkPlayer;
        
        // Start is called before the first frame update
        void Start()
        //dwdwdwde
        {
            gameManager = transform.root.GetComponentInChildren<GameManager>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            networkPlayer = transform.root.GetComponent<NetworkPlayer>();
            slider = GameObject.Find("Slider").GetComponent<Slider>();
            
        }
        
        public void Relase()
        {
            isPlayerControlled = !isPlayerControlled;
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            if (gameManager.isCurrentPLayer)
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

                _rigidbody.AddRelativeForce(0, yVelocity * heightCurve.Evaluate(transform.position.y), 0,
                    ForceMode.Force);
            }
        }

        public void YMovement()
        {
            
            if ( networkPlayer.yMovement == 0 && yVelocity > 1)
            {
                
            }
            else if (yVelocity < maxYVelocity && yVelocity > -20)
            {
                yVelocity += Time.deltaTime * yThrust * networkPlayer.yMovement;
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
            if (gameManager.isCurrentPLayer)
            {
                Vector3 rotation = new Vector3(0, 0f,
                    xzRotationPower * -networkPlayer.zRotation *
                    rotationCurve.Evaluate(transform.rotation.eulerAngles.x));

                _rigidbody.AddRelativeForce(0, 0, -networkPlayer.xRotation, ForceMode.Force);

                _rigidbody.AddRelativeTorque(rotation, ForceMode.Force);
            }
        }

    }
}
