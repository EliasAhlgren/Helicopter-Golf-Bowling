﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Vector3 lastVelocity;
    float velocityTimer = 0;


    public float fragmentSize = 0.2f;
    public int fragmentsInRow = 5;
    public int fragmentsInCell= 5;
    public int fragmentsInWidth = 5;

    GameObject parentPiece;

    private void Start()
    {
        parentPiece = new GameObject("parent");
        parentPiece.transform.position = gameObject.transform.position * 0;
        parentPiece.transform.rotation = new Quaternion(0,0,0,0);
    }


    void Update()
    {
        velocityTimer += Time.deltaTime;
        if(velocityTimer >= 0.01f)
        {
            velocityTimer = 0;
            lastVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {


        explode();
    }


    void explode()
    {
        gameObject.SetActive(false);
        parentPiece.transform.position = gameObject.transform.position;
        for (int x = 0; x < fragmentsInRow; x++)
        {
            for(int y = 0; y < fragmentsInCell; y++)
            {
                for (int z = 0; z < fragmentsInWidth; z++)
                {
                    createPieces(x, y, z);
                }
            }
        }


        //Roate the parent piece to match the host
        
        parentPiece.transform.rotation = gameObject.transform.rotation;


        
    }

    void createPieces(int x, int y, int z)
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Scale the cube and place it on the world
        piece.transform.position = transform.position + new Vector3(fragmentSize * x , fragmentSize * y, fragmentSize * z) - new Vector3((fragmentSize * fragmentsInCell) / 2, (fragmentSize * fragmentsInRow) / 2 - 0.05f, (fragmentSize * fragmentsInWidth) / 2);
        piece.transform.localScale = new Vector3(fragmentSize, fragmentSize, fragmentSize);
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = fragmentSize;
        piece.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;

        //Match same speed and direction as the parent object
        piece.GetComponent<Rigidbody>().velocity = lastVelocity;

        piece.transform.parent = parentPiece.transform;
    }
}
