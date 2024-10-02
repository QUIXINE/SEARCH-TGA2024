using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class LogPool : MonoBehaviour
{
    public void ReturnToPool(GameObject logObj)
    {
        logObj.GetComponent<Rigidbody>().useGravity = false;
        logObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        logObj.transform.position = transform.position;
        logObj.transform.rotation = transform.rotation;
        logObj.GetComponent<Rigidbody>().isKinematic = true;
        logObj.GetComponent<Rigidbody>().isKinematic = false;
        
        // I want this not to be float by gravity but after isKinematic = false, it does and 
        // isKinematic = false has to be the last line so that the warning about kinematic won't come out
    }
}
