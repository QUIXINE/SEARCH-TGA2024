using UnityEngine;

public class EnableStompBranch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            other.GetComponent<StompBranch>().enabled = true;
            Destroy(gameObject);
        }
    }
}
