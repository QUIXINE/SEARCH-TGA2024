using System;
using Cinemachine;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _newFollowOffset;
    [SerializeField] private float _rotationSpeed;
    private CinemachineVirtualCamera _cinemachineVirtualCameral;
    private CinemachineTransposer _cinemachineTransposer;
    private bool _isReadyToRotate;

    private void Start()
    {
        _cinemachineVirtualCameral = FindObjectOfType<CinemachineVirtualCamera>();
        _cinemachineTransposer = _cinemachineVirtualCameral.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (_isReadyToRotate && Vector3.Distance(_cinemachineTransposer.m_FollowOffset, _newFollowOffset) >= 0.01f)
        {
            _cinemachineTransposer.m_FollowOffset = Vector3.Slerp(_cinemachineTransposer.m_FollowOffset,
                _newFollowOffset, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _isReadyToRotate = true;
        }
    }
}
