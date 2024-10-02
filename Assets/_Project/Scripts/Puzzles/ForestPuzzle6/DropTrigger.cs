using System;
using UnityEngine;

public class DropTrigger : MonoBehaviour
{
    [SerializeField] private float _speed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Woods))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,-9.8f * _speed,0);
            // other.gameObject.GetComponent<Rigidbody>(). = Vector3.zero;
        }
    }
}
