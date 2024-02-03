using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDamage : MonoBehaviour
{
    public bool IsPlayerInSafeSpot;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage) && !IsPlayerInSafeSpot)
        {
            playerTakeDamage.TakeDamage();
        }
    }
}
