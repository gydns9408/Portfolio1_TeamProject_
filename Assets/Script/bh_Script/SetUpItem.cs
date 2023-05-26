using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpItem : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody _rigid;
    public float _notKinematicTime = 0.5f;
    WaitForSeconds wait;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        wait = new WaitForSeconds(_notKinematicTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigid.isKinematic == false) 
        {
            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;
            _rigid.rotation = Quaternion.identity;
        }
    }

    public void SetUp()
    {
        _rigid.isKinematic = false;
        StopAllCoroutines();
        StartCoroutine(returnKinematic());
    }

    IEnumerator returnKinematic()
    {
        yield return wait;
        _rigid.isKinematic = true;
    }
}
