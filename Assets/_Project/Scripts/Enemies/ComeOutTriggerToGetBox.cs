using UnityEngine;

public class ComeOutTriggerToGetBox : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _clownStateManager.IsReadyToComeOutGetBox = true;
        }
    }
}
