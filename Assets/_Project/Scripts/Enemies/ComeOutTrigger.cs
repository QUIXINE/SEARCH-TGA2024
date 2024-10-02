using System;
using UnityEngine;

public class ComeOutTrigger : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    [SerializeField] private WendigoStateManager _wendigoStateManager;
    [SerializeField] private GameObject _clown;
    [SerializeField] private GameObject _wendigo;
    private void OnTriggerEnter(Collider other)
    {
        if (_clownStateManager && other.gameObject.CompareTag(TagManager.Player))
        {
            _clown.SetActive(true);
            _clownStateManager.IsReadyToComeOut = true;
        }
        else if (_wendigoStateManager && other.gameObject.CompareTag(TagManager.Player))
        {
            _wendigo.SetActive(true);
            _wendigoStateManager.IsReadyToComeOut = true;
        }
    }
}
