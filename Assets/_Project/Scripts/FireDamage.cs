using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            print("Collide");
            playerTakeDamage.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            playerTakeDamage.TakeDamage();
        }
    }
}
