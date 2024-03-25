using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CrusherController : MonoBehaviour
{
    [Tooltip("used to assign Crush Next Time back to desired value")]
    [SerializeField] private float _crushNextTimeDefault;
    [SerializeField] private float _crushNextTime;  // indicate how long the crusher will wait to repeat the action

    [SerializeField] private Vector3 _startPosition;
    [Tooltip("manually set y-axis to tell where crusher need to go")]
    [SerializeField] private Vector3 _endPosition;
    [FormerlySerializedAs("_maxDistanceDelta")]
    [Tooltip("speed of movement")]
    [SerializeField] private float _getDownSpeed;
    [SerializeField] private float _getUpSpeed;
    [SerializeField] private float _recoveryTimeDefault;
    private float _recoveryTime;    // indicate how long the crusher will wait to repeat the action
    private Vector3 _targetPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = new Vector3(transform.position.x, _endPosition.y, transform.position.z);
        _targetPosition = _endPosition;
        _recoveryTime = _recoveryTimeDefault;
        // InvokeRepeating(nameof(Crush), _startTime, _repeatTime);
    }

    private void Update()
    {
        Crush();
    }

    void Crush()
    {
        if (transform.position == _startPosition)
        {
            _crushNextTime -= Time.deltaTime;
        }
        
        if(_crushNextTime <= 0 && _recoveryTime >= 0)
        {
            if (_targetPosition == _endPosition)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _targetPosition, _getDownSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.001f)
                {
                    _recoveryTime -= Time.deltaTime;
                    if (_recoveryTime <= 0)
                    {
                        _targetPosition = _startPosition;
                        _recoveryTime = _recoveryTimeDefault;
                    }
                }
            }
            else
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _targetPosition, _getUpSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.001f)
                {
                    _crushNextTime = _crushNextTimeDefault;
                    _targetPosition = _endPosition;
                }
            }
        }
    }
}
