using System;
using UnityEngine;

namespace GameManagement
{
    public enum Axis
    {
        X,
        Y,
        Z
    }
    
    public class TrickManager : MonoBehaviour
    {
        public Vector3 startingDirection;

        public Vector3 currentDirection;
        
        private bool shouldCheckRotation;

        public Axis checkingAxis;
        
        public void SetStartingForward()
        {
            switch (checkingAxis)
            {
                case Axis.X:
                    startingDirection = transform.up;
                    break;
                case Axis.Y:
                    startingDirection = transform.forward;
                    break;
                case Axis.Z:
                    startingDirection = transform.up;
                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (checkingAxis)
            {
                case Axis.X:
                    currentDirection = transform.up;
                    break;
                case Axis.Y:
                    currentDirection = transform.forward;
                    break;
                case Axis.Z:
                    currentDirection = transform.up;
                    break;
            }        }
    }
}