using System;
using UnityEngine;

public class CrusherDamage : MonoBehaviour
{
    //Use Dot product to make crusher damage only when player is under it, maybe it doesn't work because 
    //in case player jump and touch the crusher while it doesn't reach player use only Dot product, which only
    //calculate the angle between player and itself, won't be enough

    [SerializeField] private PlayerController _playerController;
    [SerializeField] float _dotOffset;
    [SerializeField] float _dot;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // _playerController = FindObjectOfType<PlayerController>();
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
                print("Crusher Damages PlayerController");
                Rigidbody playerRb = playerTakeDamage.gameObject.GetComponent<Rigidbody>();
                playerRb.isKinematic = true;
                Collider playerCollider = playerTakeDamage.gameObject.GetComponent<CapsuleCollider>();
                playerCollider.enabled = false;
                playerTakeDamage.TakeDamage(gameObject.scene);
            }
            else if(_playerController.gameObject.TryGetComponent<CrusherCheck>(out CrusherCheck crusherCheck) && _dot > -1 + _dotOffset)
            {
                print("CrusherCheck is found");
                //Problem: before player is crushed, IsCrusherOnHead is true, but when collides w/ crusher IsCrusherOnHead shows it's false
                print("IsCrusherOnHead: " +crusherCheck.IsCrusherOnHead);

                if (crusherCheck.IsCrusherOnHead)
                {
                    print("Crusher Damages CrusherCheck");
                    //check when the crusher crushes from downside
                    Rigidbody playerRb = playerTakeDamage.gameObject.GetComponent<Rigidbody>();
                    playerRb.isKinematic = true;
                    Collider playerCollider = playerTakeDamage.gameObject.GetComponent<CapsuleCollider>();
                    playerCollider.enabled = false;
                    playerTakeDamage.TakeDamage(gameObject.scene);
                }
            }
        }
        else if (col.gameObject.TryGetComponent<ClownStateManager>(out ClownStateManager clown))
        {
            Collider collider = clown.gameObject.GetComponent<Collider>();
            Rigidbody rb = clown.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            collider.enabled = false;
            clown.enabled = false;
            clown.gameObject.SetActive(false);
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
                playerTakeDamage.TakeDamage(gameObject.scene);
            }
        }
    }
    
    //the down crusher is moving to the top, and player is stuck between 2 floor(crusher) which makes player isn't on ground while being crushed when 2 floor crash
}
