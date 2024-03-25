using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ClownComeOutState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        stateManager.TargetTrans = stateManager.InfrontDoorTrans;
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        HandleMovement(stateManager);
        SwitchState(stateManager);
    }

    private void HandleMovement(ClownStateManager stateManager)
    {
        if(stateManager.IsReadyToComeOut)
        {
            MoveOutAndChase(stateManager);
        }
        else if (stateManager.IsReadyToComeOutGetBox)
        {
            if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) <= 0.01f)
            {
                if (!stateManager.IsBoxPutDown)
                {
                    HandleTargetTransformBeforeBoxIsPutDown(stateManager);
                }
                else if (stateManager.IsBoxPutDown)
                {
                    HandleTargetTransformAfterBoxIsPutDown(stateManager);
                }
            }
            //Move and Rotate
            else if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) != 0f)
            {
                MoveNRotate(stateManager);
            }
        }
    }

    private void HandleTargetTransformBeforeBoxIsPutDown(ClownStateManager stateManager)
    {
        if (stateManager.TargetTrans.position == stateManager.InfrontDoorTrans.position)
        {
            stateManager.TargetTrans = stateManager.TableTrans;
        }
        //decrease time and change state, check if IsBoxPutDown = false so that state can be changed to SearchBoxState
        else if (stateManager.TargetTrans == stateManager.TableTrans)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, true);
            stateManager.TimeWaitToChangeState -= Time.deltaTime;
            if (stateManager.TimeWaitToChangeState <= 0)
            {
                stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
                stateManager.SwitchState(stateManager.SearchBoxState);
            }
        }
    }

    private void HandleTargetTransformAfterBoxIsPutDown(ClownStateManager stateManager)
    {
        //In front of door to inside door
        if (stateManager.TargetTrans.position == stateManager.InfrontDoorTrans.position && !stateManager.IsTableNextTarget)
        {
            stateManager.TargetTrans = stateManager.InsideDoorTrans;
            //set the time for **Inside door to in front of door** to wait for... sec. to come out again
            stateManager.TimeWaitToChangePoint = stateManager.TimeWaitToComeOutAfterBoxIsPutDown;
        }
        //Inside door to in front of door
        else if (stateManager.TargetTrans.position == stateManager.InsideDoorTrans.position)
        {
            stateManager.TimeWaitToChangePoint -= Time.deltaTime;
            if (stateManager.TimeWaitToChangePoint <= 0)
            {
                stateManager.TargetTrans = stateManager.InfrontDoorTrans;
                stateManager.IsTableNextTarget = true;
            }
        }
        //In front of door to table
        else if (stateManager.TargetTrans.position == stateManager.InfrontDoorTrans.position && stateManager.IsTableNextTarget)
        {
            stateManager.TargetTrans = stateManager.TableTrans;
            //set the time for **decrease time and change TargetTrans** to wait for... sec. to get back to in front of Door
            //set time to stay at table
            stateManager.TimeWaitToChangePoint = stateManager.TimeToStayAtTable;
            stateManager.IsTableNextTarget = false;
        }
        //decrease time and change TargetTrans, check if IsBoxPutDown = true so that clown won't change to SearchBoxState
        else if (stateManager.TargetTrans == stateManager.TableTrans)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, true);
            stateManager.TimeWaitToChangePoint -= Time.deltaTime;
            if (stateManager.TimeWaitToChangePoint <= 0)
            {
                stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
                stateManager.TargetTrans = stateManager.InfrontDoorTrans;
            }
        }
    }

    private void MoveNRotate(ClownStateManager stateManager)
    {
        if (!stateManager.MyAnimator.GetBool(stateManager.IsWalkingAnimId.ToString()))
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
        }
        //Problem: Clown rotates while is at table and rotate weirdly inside door (rotate on x/z axis instead of y-axis)
        //Move to specific position
        stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
            stateManager.TargetTrans.position, stateManager.WalkSpeed * Time.deltaTime);
        //rotate direction
        Vector3 dir = stateManager.TargetTrans.position - stateManager.transform.position;
        dir.z *= -1;
        dir.x *= -1;
        Vector3 rotation = Vector3.RotateTowards(stateManager.transform.forward, dir, stateManager.RotationSpeed * Time.deltaTime, 0f);
        stateManager.transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void SwitchState(ClownStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.PlayerTrans.position, stateManager.transform.position) <= stateManager.AttackRange)
        {
            stateManager.SwitchState(stateManager.AttackState);
        }

        //if pos != table, door btn is pushed --> target pos == inside door pos, turn off fieldOfView without flashlight
        
        //if door btn reaches the top --> switch state, target pos == 1. in front door pos, 2. btn pos, turn on fieldOfView with flashlight
        //if fieldOfView with flashlight finds player --> TakeDamage()
        if (stateManager.IsBoxPutDown && stateManager.TargetTrans != stateManager.TableTrans && stateManager.DoorButton.IsPushed)
        {
            stateManager.TargetTrans = stateManager.InsideDoorTrans;
            stateManager.ViewNotInFlashLight.enabled = false;
        }
        // Why this code will only work if IsReadyToComeOutGetBox is true
        else if (stateManager.IsBoxPutDown && stateManager.DoorButton.IsDoorAtTop)
        {
            //Hold flashlight (activate flashlight gameObj)
            //
            //
            stateManager.SwitchState(stateManager.FlashlightState);
        }
    }

    private void MoveOutAndChase(ClownStateManager stateManager)
    {
        //Move to specific position
        if (Vector3.Distance(stateManager.transform.position, stateManager.InfrontDoorTrans.position) >= 0.1f)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
            stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
                stateManager.TargetTrans.position, stateManager.WalkSpeed * Time.deltaTime);
            stateManager.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, true);
        }

        //decrease time and change state
        stateManager.TimeWaitToChangeState -= Time.deltaTime;
        if (stateManager.TimeWaitToChangeState <= 0)
        {
            stateManager.SwitchState(stateManager.ChaseState);
        }
    }
}