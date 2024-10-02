using UnityEngine;

public sealed class RotationValueClimbLadderDown : MonoBehaviour
{
    public float YRotationValue => _yRotationValue;
    [SerializeField] private float _yRotationValue;
}
