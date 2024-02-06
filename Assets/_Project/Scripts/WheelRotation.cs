using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WheelRotation : MonoBehaviour
{
    [Tooltip("angle size to rotate, or speed to rotate, and direction of rotation (+ and -)")]
    [FormerlySerializedAs("rotateAngle")] [SerializeField] private float _rotateAngle;
    void Update()
    {
        transform.Rotate(0, 0, _rotateAngle * Time.deltaTime);
    }

    //Try use delegate to store function of cabin and wheel rotation, and put these logic in another class to use it to stop rotation at the same time
    //and to (clean code) single resposibility the code
    /* [SerializeField] private float stopRotationTime = 0;
    [SerializeField] private float nextStopRotationTime = 0;
    [SerializeField] private Transform raycastTransform;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask groundLayer;


    private void Update() 
    {
        if(stopRotationTime <= 0)
        {
            RotateWheel();
            if(nextStopRotationTime >= 0)
            {
                nextStopRotationTime -= Time.deltaTime;
            }
        }
        else if(stopRotationTime > Time.time)
        {
            stopRotationTime -= Time.deltaTime;

            if(stopRotationTime <= 0)
            {
                nextStopRotationTime = Time.time + 5;   //used this so that it will not stop while the wheel is rotating and already stop but raycast still detects the ground
            }
        }
    }

    private void RotateWheel()
    {
        transform.Rotate(0, 0, rotateAngle * Time.deltaTime );  
        //stop rotate by checking if raycast detects the ground, and setting stopRotationTime to desired time
        if(IsGroundFounded() && nextStopRotationTime <= 0)
        {
            stopRotationTime = Time.time + 3;
        }
    }

    //Check if raycast found ground layer
    private bool IsGroundFounded()
    {
        if(Physics.Raycast(raycastTransform.position, Vector3.left, raycastDistance, groundLayer) || Physics.Raycast(raycastTransform.position, Vector3.left, raycastDistance, groundLayer))
        return true;

        return false;
    } */
}
