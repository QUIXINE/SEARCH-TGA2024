using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour 
{
    private void OnTriggerEnter(Collider col) 
    {
        if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            print("collider");
            //player float
            playerTakeDamage.TakeDamage(gameObject.scene);
        }
    }    
}