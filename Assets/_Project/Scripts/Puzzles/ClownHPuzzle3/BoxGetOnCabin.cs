using System;
using UnityEngine;

public class BoxGetOnCabin : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Transform _boxParent;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Lift) 
            && col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation))
        {
            print("Collide");
            if (transform.position.y < col.collider.bounds.max.y)
            {
                transform.position = new Vector3(transform.position.x, col.collider.bounds.max.y,
                    transform.position.z);
            }
            transform.parent = col.transform.parent;

            if (col.transform.parent != null && !col.transform.parent.parent)
            {
                print("Collide Parent");
                transform.parent = col.transform.parent;
            }
        }
        else if (col.gameObject.CompareTag(TagManager.Lift)
                 && !col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation2))
        {
            transform.parent = col.transform.parent;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Lift)
            && col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation))
        {
            // Problem : the box floats in the air after getting off the cabin floor
            // _rb.isKinematic = true;
            // Vector3 pos = transform.position;
            if (transform.parent && !transform.parent.parent)
            {
                print("Exit");
                transform.parent = null;
            }
            transform.parent = null;

            
            /*print(pos);
            transform.localPosition = pos;
            print(transform.localPosition);
            _rb.isKinematic = false;*/
        }

        else if (col.gameObject.CompareTag(TagManager.Lift)
            && !col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation2))
        {
            transform.parent = null;
        }
    }
}
