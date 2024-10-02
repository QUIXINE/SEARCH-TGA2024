using System;
using UnityEngine;

public class IsBoxPassedBridgeCheck : MonoBehaviour
{
    [SerializeField] private BoxesLoop _boxesLoop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.MovableItem))
        {
            _boxesLoop.IsBoxPassedBridge = true;
        }
    }
}
