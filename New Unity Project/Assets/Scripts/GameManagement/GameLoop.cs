using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLoop : MonoBehaviour
{
    public float timeToRelase = 5;

    public UnityEvent relaseEvent;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(relaseTimer());
    }

    public IEnumerator relaseTimer()
    {
        yield return new WaitForSeconds(timeToRelase);
        relaseEvent.Invoke();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
