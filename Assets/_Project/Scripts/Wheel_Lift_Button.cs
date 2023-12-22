using UnityEngine;

public class Wheel_Lift_Button : MonoBehaviour, IButton
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 downPosition, upPosition;
    private bool isWheelAbleToMove;
    private Vector3 targetPosition;


    private void Start() 
    {
        AssignObjectToMove();
    }

    private void AssignObjectToMove()
    {
        if(wheel != null)
        {
            downPosition = new Vector3(wheel.transform.position.x, -2.22f, wheel.transform.position.z);
            upPosition = new Vector3(wheel.transform.position.x, wheel.transform.position.y, wheel.transform.position.z);
            targetPosition = downPosition;
        }
    }


    private void Update() 
    {
        MoveWheel();
    }

    private void MoveWheel()
    {
        //How can I code to not let this condition always true
       if(isWheelAbleToMove)
        {
            wheel.transform.position = Vector3.MoveTowards(wheel.transform.position ,  targetPosition, speed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(wheel.transform.position.y == targetPosition.y)
            {
                isWheelAbleToMove = false;
            }
        }
    }

    public void Execute()
    {
        //assign target position
        if(wheel.transform.position == upPosition)
        {
            targetPosition = downPosition;
        }
        else if(wheel.transform.position != upPosition && targetPosition == downPosition)
        {
            targetPosition = upPosition;
        }
        else if(wheel.transform.position == downPosition)
        {

            targetPosition = upPosition;
        }
        else if(wheel.transform.position != downPosition && targetPosition == upPosition)
        {
            targetPosition = downPosition;
        }
        
        if(!isWheelAbleToMove)
        isWheelAbleToMove = true;
    }

    /*4 conditions
        1. if isWheelAbleToMove = true, current pos = up, target = down
        2. push again: current pos < up, current target = down, target = up
        3. current pos = down, target = down
        4. push again: current pos > down, current target = up, target = down
     */
}