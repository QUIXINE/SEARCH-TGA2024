using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _zAxisPos;
    private float _horizontalInput;
    public bool IsRolled;
    public bool IsCollidedSlidingFloor;
    private void Update()
    {


        ReceiveInput();
        CheckAfterCollision();
        Move();

    }

    private void ReceiveInput()
    {
        if (IsRolled && !IsCollidedSlidingFloor)
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
        }
    }
    
    private void CheckAfterCollision()
    {
        if (IsCollidedSlidingFloor)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zAxisPos);
            IsRolled = false;
        }
    }


    private void Move()
    {
        if (!_rb.isKinematic)
        {
            _rb.velocity = new Vector3(IsCollidedSlidingFloor? -_rollSpeed * Time.deltaTime : _horizontalInput <= 0? _horizontalInput * _rollSpeed * Time.deltaTime : 0, -9.8f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Slided Trigger")
        {
            _rb.isKinematic = false;
            IsCollidedSlidingFloor = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<WendigoTakeDamage>(out WendigoTakeDamage takeDamage))
        {
            takeDamage.TakeDamage();
            gameObject.SetActive(false);
        }
    }
}
