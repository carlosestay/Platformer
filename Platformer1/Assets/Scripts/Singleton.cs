using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modified by Carlos Estay
//carlos.estay@gmail.com
//https://github.com/cestayoyarzo1
//Originally obtained from
//http://wiki.unity3d.com/index.php/Singleton
//February 22nd, 2020
//This script contains the mother class for a Singleton, which could be persistent or non-persistent,
//according to the SingletonType enumeration, which should be set in the Child Constructor

public enum SingletonType
{
    Persistent,
    NonPersitent,
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object _LockedObject = new object();
    private static bool _Destroyed = false;
    private static T _Instance;
    [SerializeField]
    protected static SingletonType _SingletonType;

    //private void Awake()
    //{
    //    if (_Instance != null) //An instance already exists!
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public static T Instance
    {
        get
        {
            if (_Destroyed)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            //suspend all other threads until this lock code area is done. It cannot be interrupted
            lock (_LockedObject)
            {
                //Try to find the instance
                _Instance = (T)FindObjectOfType(typeof(T));

                if (_Instance == null) //Instance was not found, create one
                {
                    GameObject singletonObject = new GameObject();  //Create new GameObject
                    _Instance = singletonObject.AddComponent<T>();  //Add Singleton Class to it
                    singletonObject.name = typeof(T).ToString() + "_Singleton";  //Conatenate name of the script with "_Singleton"

                    //Make Singleton persistence through scenes.
                    if (_SingletonType == SingletonType.Persistent)
                    {
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _Instance;
            }
        }
    }

    //Destroy instance if necessary
    private void OnApplicationQuit()
    {
        ChildDestroy();
        _Destroyed = true;
    }

    private void OnDestroy()
    {
        if (!_Destroyed)
        {
            ChildDestroy();
            _Destroyed = true;
        }
    }

    //This allows to do something in the child before destroying it, for instance unsubscribe from events
    virtual public void ChildDestroy()
    {

    }
}
