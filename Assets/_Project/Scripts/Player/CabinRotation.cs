using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;

public class CabinRotation : MonoBehaviour 
{
    //Try use delegate to store function of cabin and wheel rotation, and put these logic in another class to use it to stop rotation at the same time
    //and to (clean code) single responsibility the code
    [SerializeField] private float stopRotationTime = 0;
    [SerializeField] private float nextStopRotationTime = 0;
    [SerializeField] private Transform raycastTransform;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject wheel;
    [SerializeField] private WheelRotation wheelRotation;

    private void Start() 
    {
        wheelRotation = wheel.GetComponent<WheelRotation>();
    }
    
    private void Update() 
    {
        if(stopRotationTime <= 0)
        {
            // print("start rotation");
            RotateCabin();
            if(!wheelRotation.enabled)
                wheelRotation.enabled = true;
            if(nextStopRotationTime >= 0)
                nextStopRotationTime -= Time.deltaTime;
        }
        else if(stopRotationTime >= 0)
        {
            // print("stop rotation");
            wheelRotation.enabled = false;
            stopRotationTime -= Time.deltaTime;

        }
        
       /*  else if(stopRotationTime <= Time.time)
        {
            print("start rotation");
            wheel_Rotation.enabled = true;
            RotateCabin();
            if(nextStopRotationTime >= Time.time)
            {
                nextStopRotationTime -= Time.deltaTime;
            }
        } */
    }

    private void RotateCabin()
    {
        transform.localRotation = Quaternion.FromToRotation(-transform.parent.up, Vector3.down);  
        //wheel_Rotation.enabled = true;
        //stop rotate by checking if raycast detects the ground, and setting stopRotationTime to desired time
        if(IsGroundFounded() && nextStopRotationTime <= 0)
        {
            stopRotationTime = 3;
            nextStopRotationTime = 5;   //used this so that it will not stop while the wheel is rotating and already stop but raycast still detects the ground
        }
    }

    //Check if raycast found ground layer
    //Shoukd check trigger collider at to surface of ground instead because the cabin has to stop at the surface and it's easier (if don't need nextStopRotationTime)
    private bool IsGroundFounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(raycastTransform.position, Vector3.left, out hit, raycastDistance, groundLayer) || Physics.Raycast(raycastTransform.position, Vector3.right, out hit, raycastDistance, groundLayer))
        {
            if(hit.collider.CompareTag(TagManager.FerrisStopDestination))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(raycastTransform.position, new Vector3(raycastTransform.position.x + raycastDistance, raycastTransform.position.y, raycastTransform.position.z));
        Gizmos.DrawLine(raycastTransform.position, new Vector3(raycastTransform.position.x - raycastDistance, raycastTransform.position.y, raycastTransform.position.z));
    }

    /* private void LateUpdate() 
    {
        transform.localRotation = Quaternion.FromToRotation(-transform.parent.up, Vector3.down);  
    } */
}