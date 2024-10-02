using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

public class SecretDoorButton : MonoBehaviour, IButton
{
    public bool IsPushed;
    [FormerlySerializedAs("IsReachDestination")] public bool IsDoorReachDestination;
    public bool IsDoorAtTop;
    [SerializeField] private GameObject _door;
    [SerializeField] private Renderer _buttonRenderer;
    [SerializeField] private Vector3 _upPosition;
    [SerializeField] private Vector3 _downPosition;
    [SerializeField]  private float _doorMoveSpeed;
    private Vector3 _targetPosition;
    private bool _isDoorReadyToMove;

    private void Start()
    {
        _upPosition = new Vector3(_door.transform.localPosition.x, _upPosition.y, _door.transform.localPosition.z);
        _downPosition = new Vector3(_door.transform.localPosition.x, _downPosition.y, _door.transform.localPosition.z);
        _isDoorReadyToMove = true;
    }

    private void Update()
    {
        if (IsPushed)
        {
            _door.transform.localPosition = Vector3.MoveTowards(_door.transform.localPosition, _targetPosition, _doorMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(_door.transform.localPosition, _targetPosition) <= 0.001f)
            {
                IsDoorReachDestination = true;
                _buttonRenderer.material.EnableKeyword("_EMISSION");
                _isDoorReadyToMove = true;
                IsPushed = false;
            }
        }

        if (Vector3.Distance(_door.transform.localPosition, _upPosition) <= 0.001f)
        {
            IsDoorAtTop = true;
        }
        else
        {
            IsDoorAtTop = false;
        }
    }

    public void Execute()
    {
        if (_isDoorReadyToMove && !IsPushed)
        {
            IsDoorReachDestination = false;
            if (_door.transform.localPosition == _downPosition)
            {
                _targetPosition = _upPosition;
            }
            else
            {
                _targetPosition = _downPosition;
            }
            IsPushed = true;
            _buttonRenderer.material.DisableKeyword("_EMISSION");
            _isDoorReadyToMove = false;
        }
        //get up, down, each pushes 1 time
    }
}
