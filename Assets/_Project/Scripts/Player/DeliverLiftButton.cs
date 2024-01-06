using UnityEngine;

public class DeliverLiftButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject deliverLift;    
    [SerializeField] private float speed;
    [SerializeField] private float moveNextTime;
    [SerializeField] private Vector3 downPosition, upPosition;
    private bool _isNextTimeDecreased;
    private bool _isWheelAbleToMove;
    private Vector3 _targetPosition;
    private Renderer _buttonRenderer;

    private void Start() 
    {
        AssignObjectToMove();
        _buttonRenderer = transform.parent.gameObject.GetComponent<Renderer>();
    }

    private void AssignObjectToMove()
    {
        if(deliverLift != null)
        {
            downPosition = new Vector3(deliverLift.transform.position.x, deliverLift.transform.position.y, deliverLift.transform.position.z);
            upPosition = new Vector3(deliverLift.transform.position.x, 3.132f, deliverLift.transform.position.z);
            _targetPosition = upPosition;
        }
    }


    private void Update() 
    {
        MoveWheel();
    }

    private void MoveWheel()
    {
        //How can I code to not let this condition always true
        if(_isWheelAbleToMove && _targetPosition == upPosition)
        {
            deliverLift.transform.position = Vector3.MoveTowards(deliverLift.transform.position ,  _targetPosition, speed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(deliverLift.transform.position.y == upPosition.y)
            {
                //assign time, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                if(!_isNextTimeDecreased)
                {
                    moveNextTime = 4;
                    _isNextTimeDecreased = true;
                }
                else
                {
                    if(moveNextTime >= 0)
                    {
                        moveNextTime -= Time.deltaTime;
                    }
                    else if(moveNextTime <= 0)
                    {
                        //time check, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                        _targetPosition = downPosition;
                        _isNextTimeDecreased = false;
                    }
                }
            }
        }
        else if(_isWheelAbleToMove && _targetPosition == downPosition)
        {
            deliverLift.transform.position = Vector3.MoveTowards(deliverLift.transform.position ,  _targetPosition, speed * Time.deltaTime);
            if(deliverLift.transform.position.y == downPosition.y)
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
            if(deliverLift.transform.position == downPosition)
            {
                //turn off the light of btn
                _buttonRenderer.material.DisableKeyword("_EMISSION");
                _targetPosition = upPosition;
            }
            _isWheelAbleToMove = true;
        }
    }

    //Use time, if at upPos, time is 4 sec (desired time for lift to stop), after 4 sec, target posiotion is downPos,
    //if at downPos player can push btn to move the lift
}