using System;
using UnityEngine;

public class PigController : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private float _moveSpeed;
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(_destination.position.x, transform.position.y, transform.position.z),
            _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.Woods))
        {
            print("Collide");
            Rigidbody[] rigidbodies = col.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
            }

            enabled = false;
        }
    }
}
