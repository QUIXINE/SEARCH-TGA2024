using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapDamage : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            _animator.SetBool(("IsClose"), true);
            playerTakeDamage.TakeDamage();
        }
    }
}
