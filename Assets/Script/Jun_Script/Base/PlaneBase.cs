using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBase : BasementObject
{
    Rigidbody rigid;
    CapsuleCollider capsule;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }
}
