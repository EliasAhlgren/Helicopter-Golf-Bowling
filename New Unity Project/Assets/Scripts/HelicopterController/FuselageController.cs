using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuselageController : MonoBehaviour
{
    public bool isPlayerControlled = true;
    
    private Rigidbody _rigidbody;

    public float rotSpeed = 1;

    public GameManager gameManager;

    public float helicopterHealth = 100f;

    public float collisionDamageMultiplier = 2;

    public float framesOnGround;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        helicopterHealth -= other.relativeVelocity.magnitude * collisionDamageMultiplier;
        
        if (!isPlayerControlled)
        {
            gameManager.ResetFromPos(transform.position, gameObject);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        framesOnGround = 0;
    }

    private void OnCollisionStay(Collision other)
    {
        if (!gameManager.waitingForInput)
        {
            framesOnGround++;
            if (framesOnGround > 200)
            { 
                StartCoroutine(gameManager.HelicopterDestroyed());
            }
        }
        
    }

    
    public void Relase()
    {
        isPlayerControlled = !isPlayerControlled;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (helicopterHealth <= 0)
        {
            StartCoroutine(gameManager.HelicopterDestroyed());
        }
        
        if (isPlayerControlled)
        {
             _rigidbody.AddRelativeTorque(0, -Input.GetAxisRaw("YRotation") * rotSpeed,0,ForceMode.Force);
        }
       
    }
}
