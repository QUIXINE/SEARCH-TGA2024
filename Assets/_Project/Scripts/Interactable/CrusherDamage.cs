using UnityEngine;

public class CrusherDamage : MonoBehaviour
{
    //Use Dot product to make crusher damage only when player is under it, maybe it doesn't work because 
    //in case player jump and touch the crusher while it doesn't reach player use only Dot product, which only
    //calculate the angle between player and itself, won't be enough

    [SerializeField] private PlayerController _playerController;
    [SerializeField] float _dotOffset;
    [SerializeField] float _dot;

    private void Update()
    {
        Transform playerPos = _playerController.gameObject.transform;
        Vector3 dirPlayerToThis = (transform.position - playerPos.position).normalized;
        _dot = Vector3.Dot(playerPos.up, dirPlayerToThis);

    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            //can't use IsOnGround because if player jump while being crushed, then the condition won't be met
            if(playerController.IsOnGround && _dot > 1 + _dotOffset)
            {
                Rigidbody playerRb = playerTakeDamage.gameObject.GetComponent<Rigidbody>();
                playerRb.isKinematic = true;
                Collider playerCollider = playerTakeDamage.gameObject.GetComponent<CapsuleCollider>();
                playerCollider.enabled = false;
                playerTakeDamage.TakeDamage();
            }
            else if(_playerController.gameObject.TryGetComponent<CrusherCheck>(out CrusherCheck crusherCheck) && _dot > -1 + _dotOffset)
            {
                if (crusherCheck.IsCrusherOnHead)
                {
                    //check when the crusher crushes from downside
                    Rigidbody playerRb = playerTakeDamage.gameObject.GetComponent<Rigidbody>();
                    playerRb.isKinematic = true;
                    Collider playerCollider = playerTakeDamage.gameObject.GetComponent<CapsuleCollider>();
                    playerCollider.enabled = false;
                    playerTakeDamage.TakeDamage();
                }
            }
        }
    }

    //Check when jump
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.TryGetComponent<PlayerTakeDamage>(out PlayerTakeDamage playerTakeDamage))
        {
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            //can't use IsOnGround because if player jump while being crushed, then the condition won't be met
            if(playerController.IsOnGround && _dot > 1 + _dotOffset)
            {
                Rigidbody playerRb = playerTakeDamage.gameObject.GetComponent<Rigidbody>();
                playerRb.isKinematic = true;
                Collider playerCollider = playerTakeDamage.gameObject.GetComponent<CapsuleCollider>();
                playerCollider.enabled = false;
                playerTakeDamage.TakeDamage();
            }
        }
    }
    
    //the down crusher is moving to the top, and player is stuck between 2 floor(crusher) which makes player isn't on ground while being crushed when 2 floor crash
}
