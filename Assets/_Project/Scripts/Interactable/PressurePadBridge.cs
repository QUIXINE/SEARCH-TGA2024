using System;
using UnityEngine;

public class PressurePadBridge : MonoBehaviour
{
    [SerializeField] private GameObject _floorMechanic;
    [SerializeField] private Vector3 _getDownRotation, _getUpRotation;
    [SerializeField] private float _floorRotateSpeed;
    private bool _isPushed;
    private bool _isMovingDown;

    private void Start()
    {
        _getDownRotation    = new Vector3(_floorMechanic.transform.rotation.eulerAngles.x,
            _floorMechanic.transform.rotation.eulerAngles.y, _getDownRotation.z);
        _getUpRotation      = new Vector3(_floorMechanic.transform.rotation.eulerAngles.x,
            _floorMechanic.transform.rotation.eulerAngles.y, _getUpRotation.z);
    }

    private void Update()
    {
        RotateBridge();
    }

    private void RotateBridge()
    {
        //Get Down
        if(!_isPushed && _isMovingDown)
        {
            //Vector3 direction = new Vector3(_floorMechanic.transform.rotation.eulerAngles.x, _floorMechanic.transform.rotation.eulerAngles.y, _getDownRotation);
            Quaternion targetRotation = Quaternion.Euler(_getDownRotation);
            _floorMechanic.transform.rotation = Quaternion.Slerp(_floorMechanic.transform.rotation, targetRotation,
                Time.deltaTime * _floorRotateSpeed);
            
            //Get Down Finish
            if (Quaternion.Angle(_floorMechanic.transform.rotation, targetRotation) <= 0.01f)
            {
                _floorMechanic.transform.rotation = targetRotation;
                _isMovingDown = false;
            }
            
        }
        //Get Up
        else if(_isPushed)
        {
            //Vector3 direction = new Vector3(_floorMechanic.transform.rotation.x, _floorMechanic.transform.rotation.y, _getUpRotation);
            Quaternion targetRotation = Quaternion.Euler(_getUpRotation);
            _floorMechanic.transform.rotation = Quaternion.Slerp(_floorMechanic.transform.rotation, targetRotation,
                Time.deltaTime * _floorRotateSpeed);
            
            //Get Up Finish
            if (Quaternion.Angle(_floorMechanic.transform.rotation, targetRotation) <= 0.01f)
            {
                _floorMechanic.transform.rotation = targetRotation;
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Player))
        {
            _isPushed = true;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.Player))
        {
            _isPushed = false;
            _isMovingDown = true;
        }
    }
}
