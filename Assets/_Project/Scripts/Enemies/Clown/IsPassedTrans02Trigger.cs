using UnityEngine;

public class IsPassedTrans02Trigger : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _clownStateManager.IsPassed02 = true;
            Destroy(gameObject);
        }
    }
}
