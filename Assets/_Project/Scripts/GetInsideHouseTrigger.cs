using UnityEngine;
using UnityEngine.SceneManagement;

public class GetInsideHouseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            SceneManager.LoadScene("PlayerClownHouse");
        }
    }
}
