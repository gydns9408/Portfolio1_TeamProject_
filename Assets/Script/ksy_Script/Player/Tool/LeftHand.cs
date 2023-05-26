using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DropItem"))
        {
            Debug.Log("나 잡았다");

            DropItem pick = other.GetComponentInParent<DropItem>();
            if (pick == null)
            {
                pick = other.GetComponent<DropItem>();
                if (pick != null)
                {
                    pick.Picked();
                }
            }
            else
            {
                if (pick != null)
                {
                    pick.Picked();
                }
            }

        }
    }
}
