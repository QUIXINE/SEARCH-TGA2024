using UnityEngine;

public class SecretDoorButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject _door;
    [SerializeField] private Renderer _buttonRenderer;
    [SerializeField] private Vector3 _upPosition;
    [SerializeField] private Vector3 _downPosition;
    private Vector3 _targetPosition;
    private float _doorMoveSpeed;
    private bool _isPushed;
    private bool _isDoorReadyToMove;

    private void Start()
    {
        _upPosition = new Vector3(_door.transform.localPosition.x, _upPosition.y, _door.transform.localPosition.z);
        _downPosition = new Vector3(_door.transform.localPosition.x, _upPosition.y, _door.transform.localPosition.z);
    }

    private void Update()
    {
        if (_isPushed)
        {
            _door.transform.position = Vector3.MoveTowards(_door.transform.position, _targetPosition, _doorMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(_door.transform.localPosition, _targetPosition) <= 0.001f)
            {
                _buttonRenderer.material.EnableKeyword("_EMISSION");
                _isDoorReadyToMove = true;
                _isPushed = false;
            }
        }
    }

    public void Execute()
    {
        if (_isDoorReadyToMove && !_isPushed)
        {
            if (_door.transform.position == _downPosition)
            {
                _targetPosition = _upPosition;
            }
            else
            {
                _targetPosition = _downPosition;
            }
            _isPushed = true;
            _buttonRenderer.material.DisableKeyword("_EMISSION");
            _isDoorReadyToMove = false;
        }
        //get up, down, each pushes 1 time
    }
}
