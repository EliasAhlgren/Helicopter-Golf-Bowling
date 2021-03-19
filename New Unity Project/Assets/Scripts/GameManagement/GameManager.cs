using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameManagement;
using HelicopterController;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    public bool isOfflineGame;

    public bool isCurrentPLayer;
    
    public float timeToRelase = 5;

    public UnityEvent relaseEvent;

    public UnityEvent resetEvent;

    public UnityEvent deathEvent;

    public UnityEvent startEvent;
    
    TextMeshProUGUI tmp;
    
    public float timer;

    public bool waitingForInput = true;

    [SerializeField]private bool hasBeenDestroyed = false;

    [HideInInspector] public bool hasInvincibility;
    [HideInInspector] public bool isReseting;

    [HideInInspector] public Vector3 startingPos;
    [HideInInspector] public Quaternion startingRot;

    public GameObject mainRotor;

    public GameObject mainFuselage;
    
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        tmp = GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>();
        //timer = timeToRelase;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //StartCoroutine(relaseTimer(timeToRelase));
        
    }

    public void ResetFromPos(Vector3 pos, GameObject thisObject)
    {
        Debug.Log(thisObject + " HAS RESETED", thisObject);
        
        if (!isOfflineGame)
        {
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().text = "Press any key";
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().text = "Player In control";

            }
        }
        thisObject.transform.eulerAngles = Vector3.zero + Vector3.up * thisObject.transform.eulerAngles.y;
        thisObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        thisObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        mainRotor.GetComponent<RotorController>().yVelocity = 0;
        mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
        mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        resetEvent.Invoke();
        waitingForInput = true;
    }
    
    public IEnumerator relaseTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (GameObject.Find("TopText") && !isOfflineGame)
        {
            GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().text = "Control lost!";
            //GameObject.Find("MiddleText").SetActive(false);
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        relaseEvent.Invoke();
    }

    public IEnumerator Strike(float delay)
    {
        if (isOfflineGame) //offline game reset
        {
            isReseting = true;
            StopCoroutine(relaseTimer(timeToRelase));
            deathEvent.Invoke();
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Strike";
            }

            yield return new WaitForSeconds(delay);
            hasInvincibility = false;
            mainRotor.transform.position = Vector3.zero;
            mainFuselage.transform.position = startingPos;
            mainFuselage.transform.rotation = startingRot;
            mainFuselage.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mainFuselage.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            mainRotor.GetComponent<RotorController>().yVelocity = 0;
            mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Vehicle Destroyed";
                GameObject.Find("MiddleText").SetActive(false);
            }

            foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
            {
                VARIABLE.GetComponent<TargetScript>().ResetTransform();
            }

            isReseting = false;
        }
        else //Online game reset
        {
            isReseting = true;
            StopCoroutine(relaseTimer(timeToRelase));
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Strike";
            }
            if (GameObject.Find("TopText"))
            {
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().enabled = false;
            }
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = false;
            }
            yield return new WaitForSeconds(delay);
            hasInvincibility = false;
            mainRotor.transform.position = -Vector3.one * 69;
            mainFuselage.transform.position = -Vector3.one * 69;
            mainFuselage.transform.rotation = startingRot;
            mainFuselage.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mainFuselage.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            mainFuselage.GetComponent<Rigidbody>().isKinematic = true;
            mainRotor.GetComponent<RotorController>().yVelocity = 0;
            mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
            mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            mainRotor.GetComponent<Rigidbody>().isKinematic = true;
            if (GameObject.Find("MiddleText"))
            {
                GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>().text = "Vehicle Destroyed";
                GameObject.Find("MiddleText").SetActive(false);
            }

            if (GameObject.Find("TopText"))
            {
                GameObject.Find("TopText").GetComponent<TextMeshProUGUI>().enabled = true;
            }
            if (GameObject.Find("BottomText"))
            {
                GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>().enabled = true;
            }
            
            foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
            {
                VARIABLE.GetComponent<TargetScript>().ResetTransform();
            }
            
            GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().NextPlayerTurn();
            
            isReseting = false;
            
            
        }
        
    }
    
    public IEnumerator HelicopterDestroyed(float delay)
    {
        if (!hasBeenDestroyed)
        {
            if (isOfflineGame)
            {
                Debug.Log("Destruction");
                hasBeenDestroyed = true;
                StopCoroutine(relaseTimer(timeToRelase));
                deathEvent.Invoke();
                yield return new WaitForSeconds(delay);
                hasInvincibility = false;
                mainFuselage.transform.position = Vector3.zero;
                mainFuselage.transform.eulerAngles = Vector3.zero;
                mainFuselage.GetComponent<Rigidbody>().velocity = Vector3.zero;
                mainFuselage.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                mainRotor.GetComponent<RotorController>().yVelocity = 0;
                mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
                mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                if (GameObject.Find("MiddleText"))
                {
                    GameObject.Find("MiddleText").SetActive(false);
                }

                mainFuselage.GetComponent<FuselageController>().ResetHealth();
            }
            else
            {
                Debug.Log("Destruction");
                hasBeenDestroyed = true;
                StopCoroutine(relaseTimer(timeToRelase));
                deathEvent.Invoke();
                yield return new WaitForSeconds(delay);
                hasInvincibility = false;
                mainFuselage.transform.position = Vector3.zero;
                mainFuselage.transform.eulerAngles = Vector3.zero;
                mainFuselage.GetComponent<Rigidbody>().velocity = Vector3.zero;
                mainFuselage.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                mainRotor.GetComponent<RotorController>().yVelocity = 0;
                mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
                mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                if (GameObject.Find("MiddleText"))
                {
                    GameObject.Find("MiddleText").SetActive(false);
                }

                mainFuselage.GetComponent<FuselageController>().ResetHealth();
            }
            
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    private void OnGUI()
    {
        GUI.Box (new Rect (0,0,100,50), waitingForInput.ToString());
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
        if (isCurrentPLayer)
        {
            if (isOfflineGame)
            {
                isCurrentPLayer = true;
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
}
