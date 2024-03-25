using UnityEngine;

public class ShaperButton : MonoBehaviour, IButton
{
    public bool IsWoodReady { get; set; }
    [SerializeField] private Renderer _woodShaperButtonRdr;
    [SerializeField] private Renderer _shaperGreenLightRdr;
    [SerializeField] private Renderer _shaperRedLightRdr;
    [SerializeField] private GameObject _shaperDoor;
    [SerializeField] private ConveyorBelt _conveyorBelt;
    [Tooltip("define move up position on y-axis of Shaper Door")]
    [SerializeField] private float _yPos;
    [Tooltip("Shaper Door's speed of movement")]
    [SerializeField] private float _maxDistanceDelta;
    [Tooltip("Shaper Door's open time")]
    [SerializeField] private float _doorOpenTime;
    private float _doorOpenTimeDecreased;
    private Vector3 _startPosition, _endPosition, _targetPosition;
    private bool _isShaperDoorOpen;

    private void Start()
    {
        _startPosition = _shaperDoor.transform.localPosition;
        _endPosition = new Vector3(_shaperDoor.transform.localPosition.x, _yPos, _shaperDoor.transform.localPosition.z);
        _targetPosition = _endPosition;
    }

    private void Update()
    {
        MoveShaperDoor();
        DecreaseTime();
    }

    private void MoveShaperDoor()
    {
        if (_isShaperDoorOpen)
        {
            _shaperDoor.transform.localPosition =
                Vector3.MoveTowards(_shaperDoor.transform.localPosition, _targetPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(_shaperDoor.transform.localPosition, _targetPosition) <= 0.01f)
            {
                _conveyorBelt.IsMoving = true;
                _targetPosition = _startPosition;
                _doorOpenTimeDecreased = _doorOpenTime;
                _isShaperDoorOpen = false;
            }
        }
        else if(_targetPosition == _startPosition && _doorOpenTimeDecreased <= 0)
        {
            _shaperDoor.transform.localPosition =
                Vector3.MoveTowards(_shaperDoor.transform.localPosition, _targetPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(_shaperDoor.transform.localPosition, _targetPosition) <= 0.01f)
            {
                _targetPosition = _endPosition;
                _doorOpenTimeDecreased = 0;
            }
        }
    }

    void DecreaseTime()
    {
        if (_doorOpenTimeDecreased > 0)
        {
            _doorOpenTimeDecreased -= Time.deltaTime;
        }
    }
    public void Execute()
    {
        if(IsWoodReady)
        {
            print("pushed");
            IsWoodReady = false;
            _woodShaperButtonRdr.material.DisableKeyword("_EMISSION");
            _shaperRedLightRdr.material.DisableKeyword("_EMISSION");
            _shaperGreenLightRdr.material.EnableKeyword("_EMISSION");
            _isShaperDoorOpen = true;
        }
    }
}
