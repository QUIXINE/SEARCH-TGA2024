using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CrusherButton : MonoBehaviour, IButton
{
    [Header("Activate Crusher")]
    [SerializeField] private GameObject _crusher;
    [Tooltip("manually set y-axis to tell where crusher need to go)")]
    [SerializeField] private float _yPos;
    [SerializeField] private float _getDownSpeed;
    [SerializeField] private float _getUpSpeed;
    [SerializeField] private float _recoveryTime;
    [Tooltip("used to assign Crush Next Time back to desired value")]
    [SerializeField] private float _crushNextTime;
    [Tooltip("indicate how long the crusher will wait to repeat the action")]
    private float _crushNextTimeDecreased;
    private float _recoveryTimeDecreased;
    private Vector3 _startPosition, _endPosition, _targetPosition;
    
    //Btn
    [SerializeField] private Renderer _crusherBtnRenderer;
    private bool _isPushed;
    private bool _isPushedReady;
    void Start()
    {
        _startPosition = _crusher.transform.localPosition;
        _endPosition = new Vector3(_crusher.transform.localPosition.x, _yPos, _crusher.transform.localPosition.z);
        _targetPosition = _endPosition;
        _recoveryTimeDecreased = _recoveryTime;
    }

    private void Update()
    {
        Crush();
    }

    private void Crush()
    {
        if (_crushNextTimeDecreased >= 0 && !_isPushed && _crusher.transform.localPosition == _startPosition)
        {
            _crushNextTimeDecreased -= Time.deltaTime;
        }
        else if (_crushNextTimeDecreased <= 0 && !_isPushed && !_isPushedReady)
        {
            _isPushedReady = true;
            _crusherBtnRenderer.material.EnableKeyword("_EMISSION");
        }
        
        if (_isPushed && _crushNextTimeDecreased <= 0 && _recoveryTimeDecreased >= 0)
        {
            if (_targetPosition == _endPosition)
            {
                _crusher.transform.localPosition =
                    Vector3.MoveTowards(_crusher.transform.localPosition, _targetPosition, _getDownSpeed * Time.deltaTime);
                if (Vector3.Distance(_crusher.transform.position, _targetPosition) <= 0.001f)
                {
                    _recoveryTimeDecreased -= Time.deltaTime;
                    if (_recoveryTimeDecreased <= 0)
                    {
                        _recoveryTimeDecreased = _recoveryTime;
                        _targetPosition = _startPosition;
                    }
                }
            }
            else
            {
                _crusher.transform.localPosition =
                    Vector3.MoveTowards(_crusher.transform.localPosition, _targetPosition, _getUpSpeed * Time.deltaTime);
                if (Vector3.Distance(_crusher.transform.position, _targetPosition) <= 0.001f)
                {
                    _crushNextTimeDecreased = _crushNextTime;
                    _targetPosition = _endPosition;
                    _isPushed = false;
                }
            }
        }
    }

    public void Execute()
    {
        print("Pushed");
        if (_isPushedReady)
        {
            _isPushed = true;
            _crusherBtnRenderer.material.DisableKeyword("_EMISSION");
            _isPushedReady = false;
        }
    }
}
