using System;
using UnityEngine;

public class FixBoxPos : MonoBehaviour
{
    [SerializeField] private GameObject _floorMechanic;
    [SerializeField] private GameObject _floorMechanicStopTrigger;
    private Vector3 _pos;
    private bool _isCollideBox;
    private void Start()
    {
        _pos = transform.position;
    }

    private void Update()
    {
        if (_isCollideBox)
        {
            _floorMechanic.transform.position = _pos;
            _floorMechanicStopTrigger.SetActive(false);
            _floorMechanic.transform.rotation = Quaternion.Euler(0,0,-90);
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(TagManager.MovableItem))
        {
            _isCollideBox = true;
            _pos = new Vector3(_floorMechanic.transform.position.x, _floorMechanic.transform.position.y, _floorMechanic.transform.position.z);
        }
    }
}
