using System;
using UnityEngine;

public class CrusherCheck : MonoBehaviour
{
    public bool IsCrusherOnHead { get; private set; }
    [Header("Crusher Check")]
    [SerializeField] private Transform _boxCenter;
    [SerializeField] private Vector3 _halfExtents;
    [SerializeField] private float _boxCastDistance;

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        bool isHit = Physics.BoxCast(_boxCenter.position, _halfExtents, Vector3.up, out RaycastHit hitInfo,
            Quaternion.identity, _boxCastDistance);
        if (isHit)
        {
            if (hitInfo.collider.gameObject.CompareTag(TagManager.Crusher))
            {
                IsCrusherOnHead = true;
            }
        }
        else
        {
            IsCrusherOnHead = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCenter.position, _halfExtents/0.5f);
    }
}
