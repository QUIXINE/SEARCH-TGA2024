using UnityEngine;

public class IncreasePlayerSpeed : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            player.Speed = _speed;
            player.JumpForce = _jumpForce;
        }
    }
}
