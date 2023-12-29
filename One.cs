using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One : MonoBehaviour
{
    private static bool testBool = true;

    // Start is called before the first frame update
    void Start()
    {
        if (testBool == true)
        {
            Debug.Log("jarg");
        }
        testBool = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
