using UnityEngine;

public class WheelLiftButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 downPosition, upPosition;
    private bool _isWheelAbleToMove;
    private Vector3 _targetPosition;


    private void Start() 
    {
        AssignObjectToMove();
    }

    private void AssignObjectToMove()
    {
        if(wheel != null)
        {
            downPosition = new Vector3(wheel.transform.position.x, 20.26f, wheel.transform.position.z);
            upPosition = new Vector3(wheel.transform.position.x, wheel.transform.position.y, wheel.transform.position.z);
            _targetPosition = downPosition;
        }
    }


    private void Update() 
    {
        MoveWheel();
    }

    private void MoveWheel()
    {
        //How can I code to not let this condition always true
       if(_isWheelAbleToMove)
        {
            wheel.transform.position = Vector3.MoveTowards(wheel.transform.position ,  _targetPosition, speed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(wheel.transform.position.y == _targetPosition.y)
            {
                _isWheelAbleToMove = false;
            }
        }
    }

    public void Execute()
    {
        //assign target position
        if(wheel.transform.position == upPosition)
        {
            _targetPosition = downPosition;
        }
        else if(wheel.transform.position != upPosition && _targetPosition == downPosition)
        {
            _targetPosition = upPosition;
        }
        else if(wheel.transform.position == downPosition)
        {

            _targetPosition = upPosition;
        }
        else if(wheel.transform.position != downPosition && _targetPosition == upPosition)
        {
            _targetPosition = downPosition;
        }
        
        if(!_isWheelAbleToMove)
        _isWheelAbleToMove = true;
    }

    /*4 conditions
        1. if isWheelAbleToMove = true, current pos = up, target = down
        2. push again: current pos < up, current target = down, target = up
        3. current pos = down, target = down
        4. push again: current pos > down, current target = up, target = down
     */
}