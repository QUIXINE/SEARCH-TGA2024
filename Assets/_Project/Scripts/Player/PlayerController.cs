using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private float _horizontal;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    
    //Move Object
    private bool _isNearMovableObject;
    private Vector3 _playerYPos;
    [SerializeField] private Transform _movableObjDetectorLeft, _movableObjDetectorRight;
    [SerializeField] private Transform _movableObjParent;
    [SerializeField] private float _detectorRayDistance;

    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerYPos = new Vector3(0, transform.position.y, 0);
    }

    void Update()
    {
        Move();
        Jump();
        PushandPull();
    }
    
    private void Move()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector3((_horizontal * _speed) * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void PushandPull()
    {
        RaycastHit hit;
        _isNearMovableObject = Physics.Raycast(_movableObjDetectorLeft.position, Vector3.left, out hit,_detectorRayDistance) || Physics.Raycast(_movableObjDetectorLeft.position, Vector3.right, out hit,_detectorRayDistance)
                               || Physics.Raycast(_movableObjDetectorRight.position, Vector3.right, out hit,_detectorRayDistance)|| Physics.Raycast(_movableObjDetectorRight.position, Vector3.left, out hit,_detectorRayDistance)? true : false;
        if (_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.RightControl))
        {
            _rb.constraints |= RigidbodyConstraints.FreezePositionY;
            hit.collider.transform.parent.parent = _movableObjParent;
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;
            
        }
        else if(_isNearMovableObject && Input.GetKeyUp(KeyCode.RightControl))
        {
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _movableObjParent.DetachChildren();
        }
        else
        {
            _movableObjParent.DetachChildren();
        }
    }
}
