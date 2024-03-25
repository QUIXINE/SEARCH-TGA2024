using UnityEngine;

    public class IncreaseWendigoSpeed : MonoBehaviour
    {
        [SerializeField] private float _chaseNormalSpeed;
        [SerializeField] private float _chaseAggressivelySpeed;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Wendigo")
            {
                var wendigo = other.gameObject.GetComponent<WendigoStateManager>();
                wendigo.ChaseNormalSpeed = _chaseNormalSpeed;
                wendigo.ChaseAggressivelySpeed = _chaseAggressivelySpeed;
            }
        }
    }
