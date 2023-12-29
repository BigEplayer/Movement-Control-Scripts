using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] persistantObjects = null;

    private static bool objectsGenerated = false;

    private void Awake()
    {
        if (objectsGenerated) { return; }

        for(int i = 0; i < persistantObjects.Length; i++)
        {
            GameObject obj = Instantiate(persistantObjects[i]);
            DontDestroyOnLoad(obj);
        }

        objectsGenerated = true;
    }
}