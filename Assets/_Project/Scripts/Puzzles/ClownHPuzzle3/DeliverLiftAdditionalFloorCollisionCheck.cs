using System;
using UnityEngine;

public class DeliverLiftAdditionalFloorCollisionCheck : MonoBehaviour
{
    [SerializeField] private DeliverLiftAdditionalFloor _deliverLiftAdditionalFloor;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Lift) && col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation))
        {
            Debug.Log("Collide");
            _deliverLiftAdditionalFloor.IsMovingL = false;
            _deliverLiftAdditionalFloor.IsCollidedWithCabinFloor = true;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Lift) && col.transform.parent.gameObject.TryGetComponent(out CabinRotation cabinRotation))
        {
            Debug.Log("Collide");
            _deliverLiftAdditionalFloor.IsCollidedWithCabinFloor = false;
            _deliverLiftAdditionalFloor.IsAbleToMoveR = true;
        }
    }
}
