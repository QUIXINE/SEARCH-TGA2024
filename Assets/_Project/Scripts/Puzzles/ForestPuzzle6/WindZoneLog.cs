using System;
using UnityEngine;

public class WindZoneLog : MonoBehaviour
{
    [SerializeField] private LogPool _logPool;
    [SerializeField] private float _yPosToGetToPool;
    private void Update()
    {
        if (transform.position.y <= _yPosToGetToPool)
        {
            _logPool.ReturnToPool(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            PlayerTakeDamage playerTakeDamage = other.gameObject.GetComponent<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage(gameObject.scene);
        }
    }
}
