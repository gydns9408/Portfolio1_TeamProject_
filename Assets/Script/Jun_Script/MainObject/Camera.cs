using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform cameraTransform;

    public Vector3 CameraSet;

    private void Update()
    {
        transform.position = cameraTransform.position + CameraSet;
    }
}
