using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//File Created by Carlos Estay
//carlos.estay@gmail.com
//https://github.com/cestayoyarzo1
//February 22nd, 2020
//This script Extends the UnityEvents class to be able to accept parameters

public class CustomUnityEvent : UnityEvent<GameObject, CustomEventArgs>
{
    
}

public class CustomEventArgs
{
    public string BoardCommand { get; set; }
    public string PortName { get; set; }
    public int BaudRate;
    public bool Successful { get; set; }

    public CustomEventArgs()
    {

    }

    public CustomEventArgs(PointerEventData data)
    {

    }

    public CustomEventArgs(string command)
    {
        BoardCommand = command;
    }

    public CustomEventArgs(string name, int baudRate)
    {
        PortName = name;
        BaudRate = baudRate; 
    }

    public CustomEventArgs(bool success)
    {
        Successful = success;
    }


}
