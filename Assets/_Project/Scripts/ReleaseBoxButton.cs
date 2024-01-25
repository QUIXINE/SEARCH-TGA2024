using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ReleaseBoxButton : MonoBehaviour, IButton
{
    [SerializeField] private Rigidbody _boxRb;
    [SerializeField] private Renderer _releaseButtonRenderer;
    [SerializeField] private Renderer _woodShaperButtonRdr;
    [SerializeField] private ShaperButton _shaperButton;
    [SerializeField] private WoodPooled _woodPooled;

    public void Execute()
    {
        if (_woodPooled.IsWoodInPool)
        {
            _releaseButtonRenderer.material.DisableKeyword("_EMISSION");
            _woodShaperButtonRdr.material.EnableKeyword("_EMISSION");
            _boxRb.isKinematic = false;
            _shaperButton.IsWoodReady = true;
        }
    }
}
