using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLightEstimation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(Shader.GetGlobalFloat("_GlobalLightEstimation"));
    }
}
