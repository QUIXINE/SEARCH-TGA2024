using System;
using UnityEngine;

public class ActivateFakeFlashlightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _fakeFlashlight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _fakeFlashlight.SetActive(true);
            Destroy(gameObject);
        }
    }
}
