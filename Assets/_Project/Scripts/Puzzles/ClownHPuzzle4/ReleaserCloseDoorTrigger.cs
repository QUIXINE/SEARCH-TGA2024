using System;
using UnityEngine;

public class ReleaserCloseDoorTrigger : MonoBehaviour
{
    [SerializeField] private BoxesLoop _boxesLoop;
    [SerializeField] private ReleaserDoor _releaserDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _boxesLoop.Boxes[2] && _releaserDoor.IsDoorOpened)
        {
            print("close door");
            _releaserDoor.IsDoorOpened = false;
            _releaserDoor.IsDoorClosing = true;
            StartCoroutine(_releaserDoor.CloseDoor());
        }
    }
}
