using System;
using UnityEngine;

public class StopRockDamage : MonoBehaviour
{
    [SerializeField] private RockDamage _rockDamage;
    private bool _isCollidedWithRock;


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            print("Stop Damaging");
            _rockDamage.IsPlayerInSafeSpot = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !_isCollidedWithRock)
        {
            _rockDamage.IsPlayerInSafeSpot = false;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        Collider myCollider = col.GetContact(0).otherCollider;
        print(myCollider.gameObject);
        if (myCollider.gameObject == _rockDamage.gameObject)
        {
           print("Rock Collide");
             _isCollidedWithRock = true;
        }
    }
}
