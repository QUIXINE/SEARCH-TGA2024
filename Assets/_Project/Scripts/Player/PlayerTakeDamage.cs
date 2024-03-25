using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] private PlayerRespawn _playerRespawn;

    private void Start() 
    {
        _playerRespawn = GetComponent<PlayerRespawn>();
    }
    private void Update()
    {
        FallFromHeight();
    }

    public void TakeDamage(Scene sceneToLoad)
    {
        print("Take Damage");
        //Stop player from moving
        var playerController = GetComponent<PlayerController>();
        playerController.HorizontalInput = 0;
        playerController.enabled = false;
        var animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        
        
        //Stop taking damage
        transform.GetComponent<Rigidbody>().isKinematic = true;
        var collider = GetComponent<Collider>();
        collider.enabled = false;
        
        //Activate ragdoll here
        //
        //
        //
        
        enabled = false;
        
        //Respawn player
        StartCoroutine(_playerRespawn.Respawn(playerController, this, sceneToLoad));
    }
    
    //Fall from height
    private void FallFromHeight()
    {
        if (transform.position.y <= -100)
        {
            if (_playerRespawn.RespawnPoint != null)
            {
                //transform.position = _playerRespawn.RespawnPoint.position;
                TakeDamage(gameObject.scene);
            }
        }
    }

    //Fall into water
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4) //4 = water
        {
            transform.position = _playerRespawn.RespawnPoint.position;
        }
    }
    //Fall into hole, camera stops following player --> use black animation instead
    //Collides with thorn -> activate ragdoll
    //Cut by cutter machine -> activate ragdoll
    
    
}
