using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PressurePadDoor : MonoBehaviour
{
    [Header("Activate Secret Door")]
    [SerializeField] private GameObject _secretDoor;
    [SerializeField] private float _getUpSpeed;
    [Tooltip("manually set y-axis to tell where Secret Door need to go)")]
    [SerializeField] private float _yPos;

    private GameObject[] _dolls;
    private Vector3 _endPosition, _targetPosition;
    private int _objsCount;
    private bool _isDoorMoving;

    private void Start()
    {
        _endPosition = new Vector3(_secretDoor.transform.localPosition.x, _yPos, _secretDoor.transform.localPosition.z);
        _targetPosition = _endPosition;
    }
    private void Update()
    {
        if (_objsCount == 3)
        {
            _isDoorMoving = true;
            /*foreach (var doll in _dolls)
            {
                doll.GetComponent<Collider>().enabled = false;
            }*/
            if (_targetPosition == _endPosition)
            {
                _secretDoor.transform.localPosition =
                    Vector3.MoveTowards(_secretDoor.transform.localPosition, _targetPosition, _getUpSpeed * Time.deltaTime);
                if (Vector3.Distance(_secretDoor.transform.position, _targetPosition) <= 0.001f)
                {
                    _objsCount = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!_isDoorMoving)
        {
            if (col.gameObject.CompareTag(TagManager.MovableItem))
            {
                print("disable collider");
                col.gameObject.transform.parent = null;
                col.enabled = false;
                /*_dolls = new GameObject[2];
                for (int i = 0; i < _dolls.Length; i++)
                {
                    if (i == 0 && _dolls[i] == null)
                    {
                        _dolls[0] = col.gameObject;
                    }
                    else if (_dolls[i] == null && _dolls[i-1] != col.gameObject)
                    {
                        _dolls[i] = col.gameObject;
                    }
                }*/
                
                _objsCount++;
                // col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //col.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (!_isDoorMoving)
        {
            if (col.gameObject.CompareTag(TagManager.MovableItem))
            {
                _objsCount--;
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!_isDoorMoving)
        {
            if (col.gameObject.CompareTag(TagManager.Player))
            {
                _objsCount++;
            }
        }
    }
    
    private void OnCollisionExit(Collision col)
    {
        if (!_isDoorMoving)
        {
            if (col.gameObject.CompareTag(TagManager.Player))
            {
                _objsCount--;
            }
        }
    }
}
