using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Transform parentObject;
    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Pin")[GameObject.FindGameObjectsWithTag("Pin").Length / 2].transform;
        Vector3 arrowNewPos = new Vector3();
        arrowNewPos = parentObject.transform.position;
        arrowNewPos.y = parentObject.position.y + 1;
        transform.position = arrowNewPos;
        transform.LookAt(target);
    }
}
