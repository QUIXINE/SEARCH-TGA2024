using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampButton : MonoBehaviour, IButton
{
    [SerializeField] private Rigidbody _boxRb;
    [SerializeField] private Animator _edgeClampAnimator;

    public void Execute()
    {
        //play animation of edge clamp
        //_edgeClampAnimator.Play();
        _boxRb.useGravity = true;
    }
}
