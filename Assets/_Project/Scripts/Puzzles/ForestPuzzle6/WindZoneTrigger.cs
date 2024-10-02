using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WindZoneTrigger : MonoBehaviour
{
    [SerializeField] private WindZoneTest _windZoneTest;
    [SerializeField] private Collider _windZoneCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            print("Collide");
            _windZoneTest.enabled = true;
            _windZoneCollider.enabled = true;
        }
    }
}
