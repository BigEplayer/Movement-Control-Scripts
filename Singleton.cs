using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    //  Basically, T (the class that is becoming a singleton) must inherit a Singleton of type T.
    //  not fully understood. Could also just be where T : Monobehavior
{
    public static T Instance { get; private set; }

    public virtual void Awake() => Instance = this as T;
    //  to specify the instance will be of the chosen type
}

public abstract class SingletonPersistant<T> : Singleton<T> where T : Singleton<T> 
{ 
    public override void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}