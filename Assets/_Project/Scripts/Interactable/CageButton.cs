using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CageButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject _cageDoor;
    [FormerlySerializedAs("_cageDoorMovedYPos")]
    [Tooltip("used to add position to transform y-axis position to move the door towards")]
    [SerializeField] private float _cageDoorAdditionalYPos;
    [Tooltip("speed of movement")]
    [SerializeField] private float _maxDistanceDelta;

    [SerializeField] private Lever _lever;
    [SerializeField] private Renderer _buttonRenderer;

    private Vector3 _targetPosition;
    private bool _isPushed;
    private bool _isDoorReadyToMove;
    

    private void Update()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        //set target after cage is at target pos
        if (_lever.AreCagesReachTargetPos && !_isDoorReadyToMove)
        {
            _targetPosition = new Vector3(_cageDoor.transform.position.x, _cageDoor.transform.position.y + _cageDoorAdditionalYPos, _cageDoor.transform.position.z);
            _isDoorReadyToMove = true;
            _buttonRenderer.material.EnableKeyword("_EMISSION");
        }
        if (_isPushed && _isDoorReadyToMove)
        {
            _cageDoor.transform.position = Vector3.MoveTowards(_cageDoor.transform.position, _targetPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(_cageDoor.transform.localPosition, _targetPosition) <= 0.001f)
            {
                _isPushed = false;
            }
        }
    }

    public void Execute()
    {
        if (_isDoorReadyToMove)
        {
            _isPushed = true;
            _buttonRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
