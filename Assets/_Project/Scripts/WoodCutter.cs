using System;
using UnityEngine;

public class WoodCutter : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem))
        {
            print("saw collides wood");
            //Destructible Obj here
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            //separate ragdoll here
            playerTakeDamage.TakeDamage(gameObject.scene);
        }
    }
}
