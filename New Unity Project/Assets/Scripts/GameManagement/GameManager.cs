using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using HelicopterController;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    public float timeToRelase = 5;

    public UnityEvent relaseEvent;

    public UnityEvent resetEvent;

    public UnityEvent deathEvent;

    public UnityEvent startEvent;
    
    public TextMeshProUGUI tmp;
    
    public float timer;

    public bool waitingForInput = true;

    [SerializeField]private bool hasBeenDestroyed = false;

    public bool hasInvincibility;
    
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //timer = timeToRelase;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //StartCoroutine(relaseTimer(timeToRelase));
    }

    public void ResetFromPos(Vector3 pos, GameObject thisObject)
    {
        thisObject.transform.eulerAngles = Vector3.zero + Vector3.up * thisObject.transform.eulerAngles.y;
        thisObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        thisObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject.Find("rotorMain").GetComponent<RotorController>().yVelocity = 0;
        GameObject.Find("rotorMain").GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject.Find("rotorMain").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        resetEvent.Invoke();
        waitingForInput = true;
    }
    
    public IEnumerator relaseTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        relaseEvent.Invoke();
    }

    public IEnumerator HelicopterDestroyed(float delay)
    {
        if (!hasBeenDestroyed)
        {
            hasBeenDestroyed = true;
            deathEvent.Invoke();
            yield return new WaitForSeconds(delay);
            var Blayer = GameObject.Find("fuseFront");
            hasInvincibility = false;
            Blayer.transform.position = Vector3.zero;
            Blayer.transform.eulerAngles = Vector3.zero;    
            Blayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Blayer.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GameObject.Find("rotorMain").GetComponent<RotorController>().yVelocity = 0;
            GameObject.Find("rotorMain").GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject.Find("rotorMain").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").SetActive(false);
            }
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    public void StartFlight()
    {
        hasBeenDestroyed = false;
        startEvent.Invoke();
        waitingForInput = false;
        timer = timeToRelase;
        StartCoroutine(relaseTimer(timeToRelase));
    }

    public void logEvent(string Event)
    {
        Debug.Log(Event + " Invoked");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(HelicopterDestroyed(3));
        }
        
        if (!waitingForInput)
        {
            timer -= Time.deltaTime;
            tmp.text = "Time left: " + Mathf.Round(timer);
        }
        else
        {
            tmp.text = "Press any key";
            if (Input.anyKeyDown)
            {
                StartFlight();
            }
        }
        
    }
}
