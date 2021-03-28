using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building : MonoBehaviour
{

    public bool isMoving;
    float moveTimer = 0;
    public float moveSpeed = 25;
    void Update()
    {
        if(isMoving && moveTimer < 10f)
        {
            moveTimer += Time.deltaTime;
            gameObject.transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
        }
    }

    public void startMoving()
    {
        isMoving = true;
    }
}
