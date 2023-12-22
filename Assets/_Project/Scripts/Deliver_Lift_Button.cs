using UnityEngine;

public class Deliver_Lift_Button : MonoBehaviour, IButton
{
    [SerializeField] private GameObject deliverLift;
    [SerializeField] private float speed;
    [SerializeField] private float moveNextTime;
    [SerializeField] private Vector3 downPosition, upPosition;
    private bool isNextTimeDecreased;
    private bool isWheelAbleToMove;
    private Vector3 targetPosition;


    private void Start() 
    {
        AssignObjectToMove();
    }

    private void AssignObjectToMove()
    {
        if(deliverLift != null)
        {
            downPosition = new Vector3(deliverLift.transform.position.x, deliverLift.transform.position.y, deliverLift.transform.position.z);
            upPosition = new Vector3(deliverLift.transform.position.x, -7.1f, deliverLift.transform.position.z);
            targetPosition = upPosition;
        }
    }


    private void Update() 
    {
        MoveWheel();
    }

    private void MoveWheel()
    {
        //How can I code to not let this condition always true
        if(isWheelAbleToMove && targetPosition == upPosition)
        {
            deliverLift.transform.position = Vector3.MoveTowards(deliverLift.transform.position ,  targetPosition, speed * Time.deltaTime);
            
            //check if wheel is at target position then set isWheelAbleToMove = false to stop checking
            if(deliverLift.transform.position.y == upPosition.y)
            {
                print("lift is at up pos");
                //assign time, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                if(!isNextTimeDecreased)
                {
                    moveNextTime = 4;
                    isNextTimeDecreased = true;
                }
                else
                {
                    if(moveNextTime >= 0)
                    {
                        print("the time is assigned and decreased");
                        moveNextTime -= Time.deltaTime;
                    }
                    else if(moveNextTime <= 0)
                    {
                        //time check, then change target to down pos and if position = down pos then isWheelAbleToMove = false
                        print("change target, isNextTimeDecreased to : " + isNextTimeDecreased);
                        targetPosition = downPosition;
                        isNextTimeDecreased = false;
                    }
                }
            }
        }
        else if(isWheelAbleToMove && targetPosition == downPosition)
        {
            print("move to down position");
            deliverLift.transform.position = Vector3.MoveTowards(deliverLift.transform.position ,  targetPosition, speed * Time.deltaTime);
            if(deliverLift.transform.position.y == downPosition.y)
            {
                print("at down position");
                isWheelAbleToMove = false;
                print("isWheelAbleToMove is assigned to : " + isWheelAbleToMove);
            }
        }
    }
    public void Execute()
    {
        print("btn is pushed");
        if(!isWheelAbleToMove)
        {
            //assign target position
            if(deliverLift.transform.position == downPosition)
            {
                print("assign target position");
                //turn off the light of btn
                targetPosition = upPosition;
            }
            isWheelAbleToMove = true;
        }
    }

    //Use time, if at upPos, time is 4 sec (desired time for lift to stop), after 4 sec, target posiotion is downPos,
    //if at downPos player can push btn to move the lift
}