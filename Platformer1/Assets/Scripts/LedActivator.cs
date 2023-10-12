using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LedActivator : MonoBehaviour
{
    public const byte RED_LED = 0x4;
    public const byte YELLOW_LED = 0x2;
    public const byte GREEN_LED = 0x1;
    public const byte ALL_LED = RED_LED | YELLOW_LED | GREEN_LED;



    public static byte LedCount = 0;


    [SerializeField]
    GameObject flashlight, halo;
    Light itemLight;
    [SerializeField]
    float onIntensity;
    [SerializeField]
    UInt16 state;
    UInt16 prevState;
    [SerializeField]
    float yScaleON, yScaleOFF;
    [SerializeField]
    bool collisionActive;

    [SerializeField]
    char ledInitial;
    [SerializeField]
    string command;


    void Start()
    {
        itemLight = flashlight.GetComponentInChildren<Light>();
        itemLight.intensity = 0; 
        state = 0;
        prevState = state;
        halo.SetActive(false);
        collisionActive = false;
        LedCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            command = string.Empty;
            prevState = state;
            StartCoroutine("ToggleState");
            itemLight.intensity = state * onIntensity;
            command += ledInitial;
            if (state == 1)
            {
                halo.SetActive(true);
                transform.localScale = new Vector3(0.04f, yScaleON, 0.04f);
                command += '1';
            }
            else
            {
                halo.SetActive(false);   
                transform.localScale = new Vector3(0.04f, yScaleOFF, 0.04f);
                command += '0';
            }
            //command += '\r';

            byte mask = 0;
            switch (ledInitial)
            {
                case 'R':
                    mask = RED_LED;
                    break;
                case 'Y':
                    mask = YELLOW_LED;
                    break;
                case 'G':
                    mask = GREEN_LED;
                    break;

                default:
                    break;
            }
            if(state == 1)
            {
                LedCount |= mask;
            }
            else
            {
                LedCount &= (byte)~mask;
            }

            print("LED COUNT: " + LedCount);
            EventManager.Instance.onLedActivator.Invoke(gameObject, new CustomEventArgs(command));
            if(LedCount == ALL_LED)
            {
                EventManager.Instance.onGameEnd.Invoke(gameObject, new CustomEventArgs(true));
            }
        }
    }

    IEnumerator ToggleState()
    {
        yield return new WaitForSeconds(0.1f);
        if(state == prevState)
        {
            state ^= 1;
        }
    }

}
