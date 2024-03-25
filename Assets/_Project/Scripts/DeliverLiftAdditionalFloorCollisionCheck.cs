using UnityEngine;

public class DeliverLiftAdditionalFloorCollisionCheck : MonoBehaviour
{
    [SerializeField] private DeliverLiftAdditionalFloor _deliverLiftAdditionalFloor;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(TagManager.Lift))
        {
            Debug.Log("Collide");
            _deliverLiftAdditionalFloor.IsMovingL = false;
            _deliverLiftAdditionalFloor.IsCollidedWithCart = true;
        }
    }
}
