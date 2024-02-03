using UnityEngine;
using UnityEngine.Serialization;

public class DeliverLiftButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject _deliverLift;    
    private Renderer _buttonRenderer;
    
    [Header("Move Position")]
    [SerializeField] private float _moveSpeed;
    [Tooltip("First value of Move Next Time")]
    [SerializeField] private float _moveNextTimeDefault;
    [SerializeField] private float _moveNextTime;
    [Tooltip("define where the lift will move to on y-axis")]
    [SerializeField] private float _yPos;
    [SerializeField] private Vector3 _downPosition;
    [SerializeField] private Vector3 _upPosition;
    private bool _isNextTimeDecreased;
    private bool _isWheelAbleToMove;
    private Vector3 _targetPosition;

    private void Start() 
    {
        AssignObjectToMove();
        _buttonRenderer = transform.parent.gameObject.GetComponent<Renderer>();
    }

    private void AssignObjectToMove()
    {
        if(_deliverLift != null)
        {
            _downPosition = new Vector3(_deliverLift.transform.localPosition.x, _deliverLift.transform.localPosition.y, _deliverLift.transform.localPosition.z);
            _upPosition = new Vector3(_deliverLift.transform.localPosition.x, _deliverLift.transform.localPosition.y + _yPos, _deliverLift.transform.localPosition.z);
            _targetPosition = _upPosition;
        }
    }


    private void Update() 
    {
        MoveWheel();
    }

    private void MoveWheel()
    {
        //How can I code to not let this condition always true
        if(_isWheelAbleToMove && _targetPosition == _upPosition)
        {
            _deliverLift.transform.localPosition = Vector3.MoveTowards(_deliverLift.transform.localPosition ,  _targetPosition, _moveSpeed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(_deliverLift.transform.localPosition.y == _upPosition.y)
            {
                //assign time, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                if(!_isNextTimeDecreased)
                {
                    _moveNextTime = _moveNextTimeDefault;
                    _isNextTimeDecreased = true;
                }
                else
                {
                    if(_moveNextTime >= 0)
                    {
                        _moveNextTime -= Time.deltaTime;
                    }
                    else if(_moveNextTime <= 0)
                    {
                        //time check, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                        _targetPosition = _downPosition;
                        _isNextTimeDecreased = false;
                    }
                }
            }
        }
        else if(_isWheelAbleToMove && _targetPosition == _downPosition)
        {
            _deliverLift.transform.localPosition = Vector3.MoveTowards(_deliverLift.transform.localPosition ,  _targetPosition, _moveSpeed * Time.deltaTime);
            if(_deliverLift.transform.localPosition.y == _downPosition.y)
            {
                _buttonRenderer.material.EnableKeyword("_EMISSION");
                _isWheelAbleToMove = false;
            }
        }
    }
    public void Execute()
    {
        if(!_isWheelAbleToMove)
        {
            //assign target position
            if(_deliverLift.transform.localPosition == _downPosition)
            {
                //turn off the light of btn
                _buttonRenderer.material.DisableKeyword("_EMISSION");
                _targetPosition = _upPosition;
            }
            _isWheelAbleToMove = true;
        }
    }

    //Use time, if at upPos, time is 4 sec (desired time for lift to stop), after 4 sec, target posiotion is downPos,
    //if at downPos player can push btn to move the lift
}