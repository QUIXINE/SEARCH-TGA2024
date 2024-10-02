using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class DollFloat : MonoBehaviour
{
    [FormerlySerializedAs("_dollFloatPoint")] [FormerlySerializedAs("_waterCenterTrans")] [SerializeField] private Transform _dollFloatDestination;
    [SerializeField] private float _floatSpeed;
    [SerializeField] private float _distanceFromFloatDestination;
    [SerializeField] private float _freq;
    [SerializeField] private float _amp;
    private bool _isOnWater;
    private bool _isCollideTurnOffKinematicTrigger;

    private void Update()
    {
        //move to point
        if (_isOnWater)
        {
            if (Vector3.Distance(transform.position, _dollFloatDestination.position) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _dollFloatDestination.position,
                    _floatSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, _dollFloatDestination.position) > 0.01f
                && Vector3.Distance(transform.position, _dollFloatDestination.position) < _distanceFromFloatDestination)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * _freq) * _amp + transform.position.y, transform.position.z);
            }
        }
        //make it more like floating
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4) //4 = water
        {
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            _isOnWater = true;
        }
        else if (other.gameObject.name == "Turn Off Kinematic Trigger")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            _isCollideTurnOffKinematicTrigger = true;
        }
        else if (other.gameObject.layer == 6 && !_isCollideTurnOffKinematicTrigger) // 6 = ground
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
