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
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void ResetHealth()
    {
        helicopterHealth = 100f;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        
        Debug.Log(isPlayerControlled);
        
        if (!isPlayerControlled && !gameManager.waitingForInput)
        {
            gameManager.ResetFromPos(transform.position, gameObject);
        }
        else
        {
            helicopterHealth -= other.relativeVelocity.magnitude * collisionDamageMultiplier;
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

    void MouseRot()
    {
        Vector3 mouseFaceRot = new Vector3(0, Camera.main.transform.eulerAngles.y - 90f,0f);
                    transform.eulerAngles = mouseFaceRot;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (gameManager.waitingForInput)
        {
            //MouseRot();
        }
        
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
