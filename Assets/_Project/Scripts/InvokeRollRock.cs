using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InvokeRollRock : MonoBehaviour
{
    [SerializeField] private Rigidbody _rockRb;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _rockRb.constraints = RigidbodyConstraints.None;
            _rockRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            Destroy(this);
        }
    }
}
