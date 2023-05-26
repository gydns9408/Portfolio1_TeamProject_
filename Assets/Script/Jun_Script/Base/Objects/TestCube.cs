using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer= GetComponent<Renderer>();
        Material mat = renderer.material;
        mat.color = new Color(1,1,1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
