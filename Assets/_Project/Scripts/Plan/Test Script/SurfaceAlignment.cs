using System;
using UnityEngine;

public class SurfaceAlignment : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _time;
    [SerializeField] private float _rayDis;

    private void Update()
    {
        Alignment();
    }

    private void Alignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        Quaternion RotationRef = Quaternion.Euler(0, 0, 0);

        if (Physics.Raycast(ray, out info, _rayDis, _groundLayer))
        {
            print("hit ground");
            RotationRef = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, info.normal),
                _animCurve.Evaluate(_time));
            transform.rotation = Quaternion.Euler(RotationRef.eulerAngles.x, transform.eulerAngles.y,
                RotationRef.eulerAngles.z);
        }
    }
}
