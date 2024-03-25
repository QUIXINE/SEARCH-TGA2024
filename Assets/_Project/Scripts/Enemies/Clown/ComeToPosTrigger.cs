using UnityEngine;

public class ComeToPosTrigger : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    [SerializeField] private GameObject _fakeFlashlight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _fakeFlashlight.SetActive(false);
            _clownStateManager.IsComeToPos = true;
            Destroy(gameObject);
        }
    }
}
