using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnTestCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            var spawnTest = other.gameObject.GetComponent<SpawnTest>();
            spawnTest.SpawnDamage(gameObject.scene);
        }
    }
}
