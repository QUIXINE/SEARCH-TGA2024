using UnityEngine;

public class IsPassedTrans03Trigger : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _clownStateManager.IsPassed03 = true;
            Destroy(gameObject);
        }
    }
}