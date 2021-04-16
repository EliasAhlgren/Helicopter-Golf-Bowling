using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoRotationScript : MonoBehaviour
{
    private Vector3 _targetRot;
    public float speed;
    public float length;

    private Vector3 _pos;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _targetRot = transform.forward + Random.onUnitSphere;
        transform.LookAt(_targetRot);
        _pos = transform.position;
        //MoveTarget();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        _pos.y += Mathf.Sin(timer) * length;
        transform.position = _pos;
        
        var transformEulerAngles = transform.eulerAngles;
        transformEulerAngles.y += speed;
        transform.eulerAngles = transformEulerAngles;
        //transform.forward += (Random.onUnitSphere * length) * (Time.deltaTime * speed);
    }

    void MoveTarget()
    {
        Vector3 direction = (_targetRot - transform.forward) * speed;
        transform.LookAt(transform.forward + direction);
        Debug.Log(direction);
        if (transform.forward == _targetRot)
        {
            _targetRot = transform.forward + Random.onUnitSphere;
            transform.LookAt(_targetRot);
        }
        else
        {
            MoveTarget();
        }
    }
    
}
