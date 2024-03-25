using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FireShooterController : MonoBehaviour
{
    // Plan
    // move from point to point
    // fire drawline
    // 1st point after get back wait ... sec.

    [SerializeField] private Transform _leftPos, _rightPos;
    private Transform _targetPos;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _waitingTime;
    [SerializeField] private Vector3 _boxCastHalfExtents;
    [SerializeField] private Transform _boxCastCenter;
    private float _waitingTimeDecreased;
    private bool _isPlayerShot;

    private void Start()
    {
        _waitingTimeDecreased = _waitingTime;
        _targetPos = _leftPos;
    }

    private void Update()
    {
        Move();
        // Attack();
    }

    private void Attack()
    {
        bool isFoundPlayer = Physics.BoxCast(_boxCastCenter.position, _boxCastHalfExtents, Vector3.down,
            out RaycastHit hitInfo, Quaternion.identity, _playerLayerMask);
        if (isFoundPlayer && !_isPlayerShot)
        {
            var playerTakeDamage = hitInfo.collider.gameObject.GetComponent<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage(gameObject.scene);
            _isPlayerShot = true;
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, _targetPos.position) <= 0.01f)
        {
            if (_targetPos == _rightPos)
            {
                _waitingTimeDecreased -= Time.deltaTime;
                if (_waitingTimeDecreased <= 0)
                {
                    _targetPos = _leftPos;
                    _waitingTimeDecreased = _waitingTime;
                }
            }
            else
            {
                _targetPos = _rightPos;
            }
        }
        else
        {
            //move
            transform.position =
                Vector3.MoveTowards(transform.position, _targetPos.position, _moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            var playerTakeDamage = other.gameObject.GetComponent<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage(gameObject.scene);
            _isPlayerShot = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCastCenter.position, _boxCastHalfExtents);
    }
}
