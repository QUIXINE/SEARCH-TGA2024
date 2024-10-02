using UnityEngine;
using UnityEngine.Serialization;

public class DeliverLiftButton : MonoBehaviour, IButton
{
    public bool IsLiftAbleToMove;
    [SerializeField] private GameObject _deliverLift;    
    [SerializeField] private DeliverLiftButton _anotherDeliverLiftBtnCode;    
    [SerializeField] private Renderer _anotherDeliverLiftBtnRdr;    
    private Renderer _buttonRdr;
    
    [Header("Move Position")]
    [SerializeField] private float _moveSpeed;
    
    public float MoveNextTimeDecreased { get; private set; }
    [Tooltip("First value of Move Next Time")]
    [SerializeField] private float _moveNextTime;
    [Tooltip("define where the lift will move to on y-axis")]
    [SerializeField] private float _yPos;
    [SerializeField] private Vector3 _downPosition;
    [SerializeField] private Vector3 _upPosition;
    private bool _isNextTimeDecreased;
    [FormerlySerializedAs("_targetPosition")] public Vector3 TargetPosition;

    private void Start() 
    {
        AssignObjectToMove();
        _buttonRdr = transform.parent.gameObject.GetComponent<Renderer>();
    }

    private void AssignObjectToMove()
    {
        if(_deliverLift != null)
        {
            _downPosition = new Vector3(_deliverLift.transform.localPosition.x, _deliverLift.transform.localPosition.y, _deliverLift.transform.localPosition.z);
            //don't know why use _deliverLift.transform.localPosition.y + _yPos
            _upPosition = new Vector3(_deliverLift.transform.localPosition.x, /*_deliverLift.transform.localPosition.y +*/ _yPos, _deliverLift.transform.localPosition.z);
            TargetPosition = _upPosition;
        }
    }


    private void Update() 
    {
        MoveLift();
    }

    private void MoveLift()
    {
        //How can I code to not let this condition always true
        if(IsLiftAbleToMove && TargetPosition == _upPosition)
        {
            _deliverLift.transform.localPosition = Vector3.MoveTowards(_deliverLift.transform.localPosition ,  TargetPosition, _moveSpeed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(_deliverLift.transform.localPosition.y == _upPosition.y)
            {
                //assign time, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                if(!_isNextTimeDecreased)
                {
                    MoveNextTimeDecreased = _moveNextTime;
                    _isNextTimeDecreased = true;
                }
                else
                {
                    if(MoveNextTimeDecreased >= 0)
                    {
                        MoveNextTimeDecreased -= Time.deltaTime;
                    }
                    else if(MoveNextTimeDecreased <= 0)
                    {
                        //time check, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                        TargetPosition = _downPosition;
                        _isNextTimeDecreased = false;
                    }
                }
            }
        }
        else if(IsLiftAbleToMove && TargetPosition == _downPosition)
        {
            _deliverLift.transform.localPosition = Vector3.MoveTowards(_deliverLift.transform.localPosition ,  TargetPosition, _moveSpeed * Time.deltaTime);
            if(_deliverLift.transform.localPosition.y == _downPosition.y)
            {
                _buttonRdr.material.EnableKeyword("_EMISSION");
                _anotherDeliverLiftBtnRdr.material.EnableKeyword("_EMISSION");
                IsLiftAbleToMove = false;
                _anotherDeliverLiftBtnCode.IsLiftAbleToMove = false;
            }
        }
    }
    public void Execute()
    {
        if(!IsLiftAbleToMove)
        {
            //assign target position
            if(_deliverLift.transform.localPosition == _downPosition)
            {
                //turn off the light of btn
                _buttonRdr.material.DisableKeyword("_EMISSION");
                _anotherDeliverLiftBtnRdr.material.DisableKeyword("_EMISSION");
                TargetPosition = _upPosition;
                _anotherDeliverLiftBtnCode.TargetPosition = _upPosition;
            }
            IsLiftAbleToMove = true;
            _anotherDeliverLiftBtnCode.IsLiftAbleToMove = true;
        }
    }

    //Use time, if at upPos, time is 4 sec (desired time for lift to stop), after 4 sec, target posiotion is downPos,
    //if at downPos player can push btn to move the lift
}