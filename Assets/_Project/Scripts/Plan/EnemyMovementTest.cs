using System;
using UnityEngine;

public class EnemyMovementTest : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    float _horizontalInput;
    [SerializeField] float _speed;

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _characterController.Move(new Vector3(1 * _speed * Time.deltaTime, 0, 0));
    }
}
