using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class DollFloat : MonoBehaviour
{
    [SerializeField] private Transform _waterCenterTrans;
    [SerializeField] private float _floatSpeed;
    [SerializeField] private float _freq;
    [SerializeField] private float _amp;
    private bool _isOnWater;
    private bool _isCollideTurnOffKinematicTrigger;

    private void Update()
    {
        //move to point
        if (_isOnWater)
        {
            if (Vector3.Distance(transform.position, _waterCenterTrans.position) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _waterCenterTrans.position,
                    _floatSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, _waterCenterTrans.position) > 0.01f
                && Vector3.Distance(transform.position, _waterCenterTrans.position) < 5f)
            {
                print("move up and Down");
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * _freq) * _amp + transform.position.y, transform.position.z);
            }
        }
        //make it more like floating
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4) //4 = water
        {
            Debug.Log("Collide Water");
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
