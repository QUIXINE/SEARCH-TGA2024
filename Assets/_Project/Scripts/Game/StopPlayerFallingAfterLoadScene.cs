using UnityEngine;

public sealed class StopPlayerFallingAfterLoadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
