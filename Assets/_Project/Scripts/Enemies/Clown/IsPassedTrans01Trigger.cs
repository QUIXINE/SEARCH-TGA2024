using UnityEngine;

public class IsPassedTrans01Trigger : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _clownStateManager.IsPassed01 = true;
            Destroy(gameObject);
        }
    }
}
