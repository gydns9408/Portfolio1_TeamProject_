using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlower : MonoBehaviour
{
    Sunshine sun;

    bool isMushroom = false;

    private void Start()
    {
        sun = GameObject.Find("Directional Light").GetComponent<Sunshine>();
    }

    private void Update()
    {
        Light light = GetComponent<Light>();

        if(sun.isNight == true)
        {
            isMushroom = true;
        }
        else
        {
            isMushroom= false;
        }


        if(isMushroom == true)
        {
            light.intensity = 3.0f;   
        }
        else
        {
            light.intensity = 0.0f;
        }    
    }

    

}
