using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FirePitOpenClose : MonoBehaviour
{
    public bool IsPlayerClamped { get; set;}
    [Tooltip("define amount of x-axis position for Fire Pit to close, used to decreased or increase from start position")]
    [SerializeField] private float _xPos;
    [SerializeField] private float _closeSpeed, _openSpeed;
    private Vector3 _startPosition, _endPosition, _targetPosition;
    
    void Start()
    {
        _startPosition = transform.localPosition;
        _endPosition = new Vector3(transform.localPosition.x + _xPos, transform.localPosition.y, transform.localPosition.z);
        _targetPosition = _endPosition;
    }

    // Update is called once per frame
    void Update()
    {
        OpenClose();
    }

    private void OpenClose()
    {
        if (!IsPlayerClamped /*&& _crushNextTime <= 0 && _recoveryTime >= 0*/)
        {
            if (_targetPosition == _endPosition)
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, _targetPosition, _closeSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.001f)
                {
                    _targetPosition = _startPosition;
                    /*_recoveryTime -= Time.deltaTime;
                    if (_recoveryTime <= 0)
                    {
                        _recoveryTime = _recoveryTimeDefault;
                    }*/
                }
            }
            else
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, _targetPosition, _openSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.001f)
                {
                    //_crushNextTime = _crushNextTimeDefault;
                    _targetPosition = _endPosition;
                }
            }
        }
        else if (IsPlayerClamped)
        {
            transform.position =
                Vector3.MoveTowards(transform.localPosition, _startPosition, _closeSpeed * Time.deltaTime);
        }
    }
}
