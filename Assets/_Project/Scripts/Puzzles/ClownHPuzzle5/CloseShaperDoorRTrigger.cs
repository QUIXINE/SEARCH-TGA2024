using UnityEngine;

public class CloseShaperDoorRTrigger : MonoBehaviour
{
    [SerializeField]  private ShaperDoorRController _shaperDoorRController;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem))
        {
            print("CloseShaperDoorRTrigger Collides");
            _shaperDoorRController.IsCollidedChangerToClose = true;
            StartCoroutine(_shaperDoorRController.Close());
        }
    }
}
