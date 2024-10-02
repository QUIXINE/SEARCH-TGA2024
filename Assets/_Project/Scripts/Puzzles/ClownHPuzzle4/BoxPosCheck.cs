using System;
using UnityEngine;

public class BoxPosCheck : MonoBehaviour
{
    [SerializeField] private BoxesLoop _boxesLoop;
    [SerializeField] private float _yPosMaxFallPoint;
    private void Update()
    {
        if (transform.position.y <= _yPosMaxFallPoint)
        {
            _boxesLoop.ReturnToPool(gameObject);
        }
    }
}
