using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using UnityEngine.Networking;
using Unity.Jobs.LowLevel.Unsafe;

public class SerialHandler : MonoBehaviour
{
    [SerializeField]
    string comPortName;
    SerialPort comPort;
    [SerializeField]
    string[] portsNames;
    Thread serialThread;
    // Start is called before the first frame update
    List<byte> incoming;
    [SerializeField]
    string command;

    private void Awake()
    {
        portsNames = SerialPort.GetPortNames();
        incoming = new List<byte>();
        serialThread = new Thread(ProcessSerial);
    }

    private void TryOpenPort(GameObject sender, CustomEventArgs args)
    {
        SerialPort tempPort = new SerialPort(args.PortName, args.BaudRate);
        try
        {
            if (!tempPort.IsOpen)
            {
                comPort = tempPort;
                comPort.Open();
                comPortName = comPort.PortName;
                serialThread = new Thread(ProcessSerial);
                serialThread.Start();
                EventManager.Instance.onPortOpenResult.Invoke(gameObject, new CustomEventArgs(true));
            }
        }
        catch (Exception)
        {

            EventManager.Instance.onPortOpenResult.Invoke(gameObject, new CustomEventArgs(false));
        }

    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        EventManager.Instance.onPortTryOpen.AddListener(TryOpenPort);
        EventManager.Instance.onLedActivator.AddListener(LedFeedback);
    }

    private void LedFeedback(GameObject arg0, CustomEventArgs arg1)
    {
        comPort.Write(arg1.BoardCommand);
    }

    private void OnDestroy()
    {
        if(comPort != null && comPort.IsOpen)
        {         
            serialThread.Abort();
            comPort.Close();
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tring to open serial port");
            if (!comPort.IsOpen || comPort == null)
            {
                portsNames = SerialPort.GetPortNames();
                comPort = new SerialPort("COM24", 115200);
                comPort.Open();
                serialThread = new Thread(ProcessSerial);
                serialThread.Start();
            }
        }

    }

    private void ProcessSerial(object obj)
    {
        while(true)
        {
            if (comPort.BytesToRead > 0)
            {
                byte data = (byte)comPort.ReadByte();

                if(data == 13)
                {
                    //ProcessCommand(incoming.ToArray());
                    Dispatcher.Invoke(() =>
                    {
                        ProcessCommand(incoming.ToArray());
                    });

                    incoming.Clear();
                }
                else
                {
                    incoming.Add(data);
                }
            }
        }
    }

    void ProcessCommand(byte[] array)
    {
        command = System.Text.Encoding.UTF8.GetString(array, 0, array.Length);
        print(command);
        EventManager.Instance.onBoardInteraction.Invoke(gameObject, new CustomEventArgs(command));
    }

}
