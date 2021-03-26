using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameManagement;
using HelicopterController;
using MLAPI;
using MLAPI.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NetworkPlayer = HelicopterController.NetworkPlayer;
using Object = UnityEngine.Object;

public enum UIstate
{
    Awaiting,
    Spectator,
    Player,
    LostControl,
    Destroyed,
    Strike
}

public class GameManager : MonoBehaviour
{
    public UIstate currentUIstate;
    
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

    private TextMeshProUGUI _topText;
    private TextMeshProUGUI _middleText;
    private TextMeshProUGUI _bottomText;
    
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _topText = GameObject.Find("TopText").GetComponent<TextMeshProUGUI>();
        _middleText = GameObject.Find("MiddleText").GetComponent<TextMeshProUGUI>();
        _bottomText = GameObject.Find("BottomText").GetComponent<TextMeshProUGUI>();
        
        
        //timer = timeToRelase;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //StartCoroutine(relaseTimer(timeToRelase));
        
    }

    public void ResetFromPos(Vector3 pos, GameObject thisObject, Vector3 contact)
    {
        if (isCurrentPLayer)
        {
            currentUIstate = UIstate.Awaiting;
        }
        
        Debug.Log(thisObject + " HAS RESETED", thisObject);
        
        if (!isOfflineGame)
        {
        }
        thisObject.transform.eulerAngles = Vector3.zero + Vector3.up * thisObject.transform.eulerAngles.y;
        thisObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        thisObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        mainRotor.GetComponent<RotorController>().yVelocity = 0;
        mainRotor.GetComponent<Rigidbody>().velocity = Vector3.zero;
        mainRotor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (PlayerPrefs.GetInt("EasyMode") == 1)
        {
            thisObject.transform.up = contact;
        }
        
        resetEvent.Invoke();
        waitingForInput = true;
    }
    
    public IEnumerator relaseTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        currentUIstate = UIstate.LostControl;
        relaseEvent.Invoke();
    }

    public IEnumerator Strike(float delay)
    {
        currentUIstate = UIstate.Strike;
        if (isOfflineGame) //offline game reset
        {
            isReseting = true;
            StopCoroutine(relaseTimer(timeToRelase));
            deathEvent.Invoke();
            

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
            
            
            foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
            {
                VARIABLE.GetComponent<TargetScript>().ResetTransform();
            }

            if (GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().isHost)
            {
                GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().NextPlayerTurn();
            }
            else
            {
                transform.root.GetComponent<NetworkPlayer>().InvokeClientRpcOnClient<RpcResponse<string>>("HostNextPlayer", 0);
            }
            
            isReseting = false;
            
            
        }
        
    }
    
    public IEnumerator HelicopterDestroyed(float delay)
    {
        currentUIstate = UIstate.Destroyed;
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
                

                mainFuselage.GetComponent<FuselageController>().ResetHealth();
            }
            else //Online game destruction
            {
                isReseting = true;
            StopCoroutine(relaseTimer(timeToRelase));
            
            
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
           
            
            
            foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("Pin"))
            {
                VARIABLE.GetComponent<TargetScript>().ResetTransform();
            }

            if (GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().isHost)
            {
                GameObject.Find("ScoreManager").GetComponent<MultiplayerManager>().NextPlayerTurn();
            }else
            {
                transform.root.GetComponent<NetworkPlayer>().InvokeClientRpcOnClient<RpcResponse<string>>("HostNextPlayer", 0);
            }
            
            isReseting = false;
            
            
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
        currentUIstate = UIstate.Player;
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
        if (!isCurrentPLayer && !hasInvincibility)
        {
            currentUIstate = UIstate.Spectator;
        }

        if (transform.root.GetComponent<NetworkedObject>().IsLocalPlayer)
        {
            switch (currentUIstate)
                    {
                        case UIstate.Awaiting:
                            if (_bottomText)
                            {
                                _middleText.enabled = false;
                                _bottomText.enabled = true;
                                _bottomText.text = "Press any key";
                                _topText.enabled = true;
                                _topText.text = "Player In control";
                            }
                            break;
                        case UIstate.Player:
                            if (_bottomText)
                            {
                                _middleText.enabled = false;
                                _bottomText.enabled = true;
                                _bottomText.text = "Time left: " + Mathf.Round(timer);
                                _topText.enabled = true;
                                _topText.text = "Player In control";
                            }
                            break;
                        case UIstate.LostControl:
                            _middleText.enabled = false;
                            _bottomText.enabled = false;
                            _topText.enabled = true;
                            _topText.text = "Lost Control";
                            break;
                        case UIstate.Spectator:
                            if (_bottomText)
                            {
                                _middleText.enabled = false;
                                _bottomText.enabled = false;
                                _topText.enabled = true;
                                _topText.text = "Spectating";
                            }
                            break;
                        case UIstate.Destroyed:
                            if (_bottomText)
                            {
                                _topText.enabled = false;
                                _middleText.text = "Destroyed";
                                _bottomText.enabled = false;
                            }
                            break;
                        case UIstate.Strike:
                            _topText.enabled = false;
                            _middleText.text = "Strike";
                            _bottomText.enabled = false;
                            break;
                    }
        }
        
        
        
        if (isCurrentPLayer)
        {
            if (isOfflineGame)
            {
                isCurrentPLayer = true;
            }
            

            if (!waitingForInput)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    StartFlight();
                    
                }
            }
        }
    }
}
