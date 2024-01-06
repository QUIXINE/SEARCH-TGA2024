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
            print("Colldie");
            _animator.SetBool(("IsClose"), true);
            
            //Stop player from moving
            PlayerController playerController =
                playerTakeDamage.gameObject.GetComponent<PlayerController>();
            playerController.enabled = false;
            playerTakeDamage.TakeDamage();
        }
    }
}
