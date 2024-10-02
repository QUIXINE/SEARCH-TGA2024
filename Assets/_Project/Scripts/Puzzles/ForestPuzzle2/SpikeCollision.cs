using UnityEngine;

public class SpikeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.Player))
        {
            col.gameObject.GetComponent<PlayerTakeDamage>().TakeDamage(gameObject.scene);
        }
    }
}
