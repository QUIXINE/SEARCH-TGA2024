using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGetOnLift : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
 
    
    private void OnCollisionEnter(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = col.gameObject.transform.parent;
            _rb.velocity = Vector3.zero;
        }    
    }

    private void OnCollisionExit(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = null;
            _rb.velocity = Vector3.zero;
        }    
    }
}
