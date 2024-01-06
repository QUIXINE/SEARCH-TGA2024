using UnityEngine;

public class StopRollingRock : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isCollidedWithRockStop;
    [SerializeField] private float _rollingAngle;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _movePosition;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _deceleratingSpeed;
    [SerializeField] private float _decreaseSpeedNextTime;
    [SerializeField] private float _deSpeedNextTimeDuration;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RollRockAfterReachStop();
    }

    private void RollRockAfterReachStop()
    {
        if (_isCollidedWithRockStop)
        {
            transform.Rotate(_rollingAngle * _rollSpeed * Time.deltaTime,0,0, Space.Self);
            transform.Translate(0,0,_movePosition * _moveSpeed * Time.deltaTime, Space.World);
            if (_decreaseSpeedNextTime <= 0)
            {
                _decreaseSpeedNextTime = Time.time;
            }
            else if (Time.time >= _decreaseSpeedNextTime)
            {
                _moveSpeed -= _deceleratingSpeed;
                _decreaseSpeedNextTime += _deSpeedNextTimeDuration;
            }
            if (transform.position.z >= 1.5 || _moveSpeed <= 0)
            {
                _isCollidedWithRockStop = false;
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Rock Stop")
        {
            print(("Collide"));
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionZ & ~RigidbodyConstraints.FreezeRotationX;
            _isCollidedWithRockStop = true;
        }
    }
}
