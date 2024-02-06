using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    public void TakeDamage()
    {
        var playerController = GetComponent<PlayerController>();
        playerController.enabled = false;
        enabled = false;
    }
}
