using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    
    private float hAxis;
    private float vAxis;

    Rigidbody rigid;

    Vector3 moving;

    //Animator anim;

    private void Awake()
    {
        Debug.Log("Player");
        rigid = GetComponent<Rigidbody>();
        //anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // moving = new Vector3(hAxis, 0, vAxis).normalized;
        rigid.AddForce(new Vector3(hAxis, 0, vAxis), ForceMode.Impulse);

        // transform.position += Time.deltaTime * moving * speed;

        // anim.SetBool("isWalk", moving != Vector3.zero);
    }
}
