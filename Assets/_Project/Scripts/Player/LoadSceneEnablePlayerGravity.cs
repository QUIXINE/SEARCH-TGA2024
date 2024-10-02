using System;
using UnityEngine;

public class LoadSceneEnablePlayerGravity : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<PlayerController>().gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
