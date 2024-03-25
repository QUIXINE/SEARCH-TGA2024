using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeliverLiftAdditionalFloor : MonoBehaviour
{
    [SerializeField] private float _raycastDistance;
    [FormerlySerializedAs("_moveSpeed")] [SerializeField] private float _openSpeed;
    [SerializeField] private float _closeSpeed;
    [Tooltip("define distance on x-axis of additional floor to move close to cabin")]
    [SerializeField] private float _moveToXPos;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    [FormerlySerializedAs("_isMovingL")] public bool IsMovingL;
    private bool _isAbleToMoveR;
    [FormerlySerializedAs("_isCollidedWithCart")] public bool IsCollidedWithCart;
    [Tooltip("used to check time of deliver lift to make additional floor close when the lift is down")]
    [SerializeField] private DeliverLiftButton _deliverLiftButton;

    private void Start()
    {
        _startPosition = transform.localPosition;
        _endPosition = new Vector3(_moveToXPos, transform.localPosition.y, transform.localPosition.z);
    }

    private void Update()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(transform.position, Vector3.left, out hit, _raycastDistance);
        if(isHit && hit.collider.gameObject.CompareTag(TagManager.Lift))
        {
            IsMovingL = true;
            _isAbleToMoveR = false;
        }

        if (IsMovingL)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _endPosition, _openSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition,  _endPosition) <= 0.01f && _deliverLiftButton.MoveNextTimeDecreased <= 0)
            {
                _isAbleToMoveR = true;
                IsMovingL = false;
            }
        }
        else if (_isAbleToMoveR)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startPosition, _closeSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _startPosition) <= 0.01f)
            {
                _isAbleToMoveR = false;
            }
        }
        else if (IsCollidedWithCart && _deliverLiftButton.MoveNextTimeDecreased <= 0)
        {
            _isAbleToMoveR = true;
            IsCollidedWithCart = false;
        }
    }
    

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _raycastDistance, transform.position.y, transform.position.z));
    }
}
