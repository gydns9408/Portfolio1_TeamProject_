using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemGet : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish")) 
        {
            DropItem dropItem = collision.gameObject.GetComponent<DropItem>();
            if (dropItem != null)
            {
                dropItem.Picked();
            }
        }
    }
}
