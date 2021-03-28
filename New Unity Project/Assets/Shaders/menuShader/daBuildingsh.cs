using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daBuildingsh : MonoBehaviour
{

    public float speed = 100;
    void Update()
    {
        if(gameObject.transform.position.x > 282) 
        {
            gameObject.transform.position = new Vector3(-1623, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
