using System;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class BearTrapPosition : MonoBehaviour
{
    private Vector3 _position;
    private void Start()
    {
        _position = transform.position;
    }

    private void Update()
    {
        if (transform.position.y >= _position.y || transform.position.y <= _position.y)
        {
            _position = new Vector3(transform.position.x, _position.y, 0);
            transform.position = _position;
        }
    }
}
