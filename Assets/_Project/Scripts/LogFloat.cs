using UnityEngine;

public class LogFloat : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _waterCenterPos;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _yPosToCheckPos;
    private bool _isOnWater;

    private void Update()
    {
        if (_isOnWater)
        {
            if (Vector3.Distance(transform.position, _waterCenterPos.position) > 0.01f)
            {
                print("move to center");
                transform.position =
                    Vector3.MoveTowards(transform.position, _waterCenterPos.position, _moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _waterCenterPos.position) < 0.02f)
                {
                    var rotation = transform.rotation = Quaternion.Euler(0,0,0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
                }
            }

            if (transform.position.y <= _waterCenterPos.position.y - _yPosToCheckPos)
            {
                _rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4) // 4 = water
        {
            _isOnWater = true;
        }
    }
}
