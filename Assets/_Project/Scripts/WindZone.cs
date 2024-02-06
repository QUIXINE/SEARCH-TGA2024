using System;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] private float _windForce;
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject)
        {
            var hitObj = col.gameObject;
            var rb = hitObj.GetComponent<Rigidbody>();
            var dir = -hitObj.transform.forward;
            Vector3 rotated = Quaternion.Euler(45, 0, 0) * Vector3.one;
            float angleOfMovement = Mathf.Atan2(rotated.x, rotated.z) * Mathf.Rad2Deg;
            Quaternion currentRotation = hitObj.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(angleOfMovement, 0, 0);
            rb.AddForce(dir * _windForce);
            hitObj.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, 1 * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        var rb = col.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        
    }
}
