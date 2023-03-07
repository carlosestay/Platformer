using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//File Created by Carlos Estay
//carlos.estay@gmail.com
//https://github.com/cestayoyarzo1
//February 22nd, 2020
//This script contains the event manager based on a non persistent singleton patern

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, CustomEvent> customEvents;

    //Unity Events
    public CustomUnityEvent onPointerEnter { get; set; }
    public CustomUnityEvent onPointerExit { get; set; }
    public CustomUnityEvent onPointerDown { get; set; }
    public CustomUnityEvent onPointerUp { get; set; }
    public CustomUnityEvent onPointerClick { get; set; }
    public CustomUnityEvent onBoardInteraction { get; set; }
    public CustomUnityEvent onPortTryOpen { get; set; }
    public CustomUnityEvent onPortOpenResult { get; set; }
    public CustomUnityEvent onGameEnd { get; set; }
    public CustomUnityEvent onLedActivator { get; set; }

    //Native C# events

    //Delegate
    public delegate void CustomEvent(GameObject sender, CustomEventArgs args);

    //Events
    //public event CustomEvent onPointerEnterNative;
    //public event CustomEvent onPointerExitNative;
    //public event CustomEvent onPointerDownNative;
    //public event CustomEvent onPointerUpNative;
    //public event CustomEvent onPointerClickNative;
    //public event CustomEvent onPortTryOpen;
    //public event CustomEvent onPortOpenResult;

    //public event CustomEvent onBoardInteraction;    //Event added for detecting board commands


    public void InvokeNative(string eventName, GameObject sender, CustomEventArgs args)
    {
        CustomEvent calledEvent;
        if (!customEvents.TryGetValue(eventName, out calledEvent))
        {
            print("Event named: " + eventName + " not found");
        }
        else
        {
            calledEvent.Invoke(sender, args);
        }
    }

    //Constructor
    public EventManager()
    {
        //change this line for making the object persistent
        _SingletonType = SingletonType.Persistent;
    }

    private void Awake()
    {
        onPointerEnter = new CustomUnityEvent();
        onPointerExit = new CustomUnityEvent();
        onPointerDown = new CustomUnityEvent();
        onPointerUp = new CustomUnityEvent();
        onPointerClick = new CustomUnityEvent();
        onBoardInteraction = new CustomUnityEvent();
        onPortTryOpen = new CustomUnityEvent();
        onPortOpenResult = new CustomUnityEvent();
        onGameEnd = new CustomUnityEvent();
        onLedActivator = new CustomUnityEvent();
        Beacon();//Print a message on the console to show the EventManager has been created
    }

    private void Start()
    {
        customEvents = new Dictionary<string, CustomEvent>();
        //customEvents.Add(nameof(onPointerEnterNative), onPointerEnterNative);
        //customEvents.Add(nameof(onPointerExitNative), onPointerExitNative);
        //customEvents.Add(nameof(onPointerDownNative), onPointerDownNative);
        //customEvents.Add(nameof(onPointerUpNative), onPointerUpNative);
        //customEvents.Add(nameof(onPointerClickNative), onPointerClickNative);
        //customEvents.Add(nameof(onPortTryOpen), onPortTryOpen);
        //customEvents.Add(nameof(onPortOpenResult), onPortOpenResult);
    }

    public void Beacon()
    {
        print("Hello World from Event Manager");
    }

    public override void ChildDestroy()
    {
        base.ChildDestroy();
        //Remove all listeners before destroying the object
        print("Event Manager is about to be destroyed");
        onPointerEnter.RemoveAllListeners();
        onPointerExit.RemoveAllListeners();
        onPointerUp.RemoveAllListeners();
        onPointerDown.RemoveAllListeners();
        onPointerClick.RemoveAllListeners();
        onBoardInteraction.RemoveAllListeners();

        //Native C# events unsubscribe
        //UnsubscribeAll(onPointerEnterNative);
        //UnsubscribeAll(onPointerExitNative);
        //UnsubscribeAll(onPointerDownNative);
        //UnsubscribeAll(onPointerUpNative);
        //UnsubscribeAll(onPointerClickNative);
    }

    void UnsubscribeAll(CustomEvent eventItem)
    {
        //print(eventItem.Method.Name + " all listeners removed");
        //Delegate[] subscribers = eventItem.GetInvocationList();
        //foreach (var item in subscribers)
        //{
        //    eventItem -= item as CustomEvent;
        //}
    }
}
