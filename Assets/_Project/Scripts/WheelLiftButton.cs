using UnityEngine;
using UnityEngine.Serialization;

public class WheelLiftButton : MonoBehaviour, IButton
{
    [SerializeField] private GameObject _wheel;
    [FormerlySerializedAs("_speed")] [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 _downPosition;
    [SerializeField] private Vector3 _upPosition;
    
    [Tooltip("define where the lift will move to on y-axis")]
    [SerializeField] private float _yPos;
    private bool _isWheelAbleToMove;
    private Vector3 _targetPosition;


    private void Start() 
    {
        AssignObjectToMove();
    }

    private void AssignObjectToMove()
    {
        if(_wheel != null)
        {
            _downPosition = new Vector3(_wheel.transform.localPosition.x, _yPos, _wheel.transform.localPosition.z);
            _upPosition = new Vector3(_wheel.transform.localPosition.x, _wheel.transform.localPosition.y, _wheel.transform.localPosition.z);
            _targetPosition = _downPosition;
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
            _wheel.transform.localPosition = Vector3.MoveTowards(_wheel.transform.localPosition ,  _targetPosition, _moveSpeed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(_wheel.transform.position.y == _targetPosition.y)
            {
                _isWheelAbleToMove = false;
            }
        }
    }

    public void Execute()
    {
        //assign target position
        if(_wheel.transform.position == _upPosition)
        {
            _targetPosition = _downPosition;
        }
        else if(_wheel.transform.position != _upPosition && _targetPosition == _downPosition)
        {
            _targetPosition = _upPosition;
        }
        else if(_wheel.transform.position == _downPosition)
        {

            _targetPosition = _upPosition;
        }
        else if(_wheel.transform.position != _downPosition && _targetPosition == _upPosition)
        {
            _targetPosition = _downPosition;
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