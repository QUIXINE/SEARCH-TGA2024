using UnityEngine;
public class ClownSearchBoxState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        stateManager.TargetTrans = stateManager.ButtonTrans;
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        //Move to button position
        if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 0.001f 
            && stateManager.TargetTrans.position != stateManager.InsideSecretDoorTrans.position)
        {
            stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
                stateManager.TargetTrans.position, stateManager.WalkSpeed * Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(0,0,0);
            //rotate direction
            if (stateManager.TargetTrans.position != stateManager.BoxTrans.position)
            {
                Vector3 dir = stateManager.TargetTrans.position - stateManager.transform.position;
                dir.z *= -1;
                dir.x *= -1;
                rotation = Quaternion.LookRotation(dir);
            }
            else
            {
                rotation = Quaternion.Euler(0,-270,0);
            }
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
        }
        //Move to InsideSecretDoorPos position after the secret door reaches the top position
        else if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 0.001f
                 && stateManager.TargetTrans.position == stateManager.InsideSecretDoorTrans.position && stateManager.DoorButton.IsDoorAtTop)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
            stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
                new Vector3(stateManager.TargetTrans.position.x, stateManager.transform.position.y, stateManager.TargetTrans.position.z), stateManager.WalkSpeed * Time.deltaTime);
            //rotate direction
            /*Vector3 dir = stateManager.TargetTrans.position - stateManager.transform.position;
            dir.z *= -1;
            dir.x *= -1;
            Quaternion rotation = Quaternion.LookRotation(dir);*/
            Quaternion rotation = Quaternion.Euler(0,270,0);
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
        }
        
        
        //Arrive at button position
        if (Vector3.Distance(stateManager.transform.position, stateManager.ButtonTrans.position) <= 0.01f && stateManager.TimeWaitToPressBtn >= 0)
        {
            //stop walking, playing idle animation
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, true);
            
            //Time wait to play press-btn-animation before invoke Execute() of button
            //if doesn't use time the door will open after reach the BtnPos immediately 
            stateManager.TimeWaitToPressBtn -= Time.deltaTime;
            
            //Rotate to look at secret door
            Vector3 dir = stateManager.InsideSecretDoorTrans.position - stateManager.transform.position;
            dir.z *= -1;
            dir.x *= -1;
            Quaternion rotation = Quaternion.LookRotation(dir);
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
            
        }
        //Press Button
        else if (stateManager.TimeWaitToPressBtn < 0 && !stateManager.DoorButton.IsDoorAtTop && !stateManager.DoorButton.IsPushed)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsPressingButtonAnimId, true);
            stateManager.DoorButton.Execute();
        }
        //if anim finishes, change target
        else if (!stateManager.IsBoxPutDown && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("PressButton") 
            && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !stateManager.MyAnimator.IsInTransition(0))
        {
            stateManager.MyAnimator.SetBool(stateManager.IsPressingButtonAnimId, false);
            stateManager.TargetTrans = stateManager.InsideSecretDoorTrans;
            // stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
        }
        //Search box and carry it
        if (Physics.Raycast(stateManager.transform.position, -stateManager.transform.forward, out RaycastHit Info, stateManager.RayToBoxDistance)
            && !stateManager.IsBoxPutDown)
        {
            if (Info.collider.gameObject.CompareTag(TagManager.MovableItem))
            {
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithCrateAnimId, true);

                //reposition box to hands position
                Info.collider.gameObject.transform.parent.position = stateManager.HoldTrans.position;
                Info.collider.gameObject.transform.parent.parent = stateManager.HoldTrans;
                
                //assign hold point to right hand as parent
                stateManager.HoldTrans.parent = stateManager.RightHandTrans;
                
                //get to BoxPos, after get there put the box down, goes to the room, after that loop come out state
                stateManager.TargetTrans = stateManager.BoxTrans;
            }
            
        }
        
        
        //play put down box animation, change target transform to btn pos after bos is put down
        if (stateManager.TargetTrans == stateManager.BoxTrans && Vector3.Distance(stateManager.transform.position, stateManager.BoxTrans.position) <= 0.01f)
        {
            //put the box down's code is here
            //
            //1. IsBoxPutDown = true is assigned in SnapBoxToGround.cs
            //2. Check if animation is finished && IsBoxPutDown, or check the time (decrease time after IsBoxPutDown to wait animation to finish,
            //   so the time has to be related with the animation time) && IsBoxPutDown 
            //3. check if animation is finished then set the TimeWaitToChangeState to decrease it and when it's <= 0 then TargetTrans can be changed.
            //   Do this to make a gap of time so that the clown won't move to the target position immediately and not naturally
            //4. if IsBoxPutDown and animation is finished, change TargetTrans like below
            
            /*//for testing
                GameObject boxParent = stateManager.HoldTrans.GetChild(0).gameObject;
                boxParent.transform.parent = null;
                Rigidbody rb = boxParent.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                stateManager.IsBoxPutDown = true;
                stateManager.TimeWaitToChangeState = 0;
                stateManager.TimeWaitToPressBtn = 3.5f;
                stateManager.TargetTrans = stateManager.ButtonTrans;
            //*/

            //assign put down box animation
            if (!stateManager.MyAnimator.GetBool(stateManager.IsPuttingDownAnimId.ToString()))
            {
                stateManager.MyAnimator.SetBool(stateManager.IsPuttingDownAnimId, true);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithCrateAnimId, false);
            }
            
            //check if put down box animation is finished
            if (stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("PutDown") 
                && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !stateManager.MyAnimator.IsInTransition(0)
                && stateManager.TimeWaitToChangeState <= 0)
            {
                //assign TimeWaitToChangeState as the way to use to check the else if below
                stateManager.TimeWaitToChangeState = 1;
            }
            //check if IsBoxPutDown before changing the target transform, and setting button time after reaching BoxTrans and putting down the box
            else if (stateManager.IsBoxPutDown && stateManager.TimeWaitToChangeState >= 0)
            {
                stateManager.MyAnimator.SetBool(stateManager.IsPuttingDownAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
                stateManager.TargetTrans = stateManager.ButtonTrans;
                stateManager.TimeWaitToPressBtn = 5;
            }
            
        }
        //After box is put down
        //Press button, if secret door is at top position (after box is put down)
        else if (Vector3.Distance(stateManager.transform.position, stateManager.ButtonTrans.position) <= 0.01f
                 && stateManager.TargetTrans.position == stateManager.ButtonTrans.position && stateManager.DoorButton.IsDoorAtTop && stateManager.IsBoxPutDown/*&& stateManager.TimeWaitToPressBtn < 0*/)
        {
            //TimeWaitToPressBtn is decreased inside "Arrive at button position" and Idle animation is set in it too
            //TimeWaitToPressBtn should be set like if TimeWaitToPressBtn = 5, then condition will be: if < 4 press btn (play animation), if < 1 change target transform
            //set < 4 for waiting the clown to get to btn position a little bit so that it won't look like the clown is in hurry and so that the animation would be displayed smoothly
            //set < 1 for waiting the animation to play/stop completely then change target transform to move out

            if (stateManager.TimeWaitToPressBtn is < 4 and > 3)
            {
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsPressingButtonAnimId, true);
                stateManager.DoorButton.Execute();
            }
        }
        else if (stateManager.TimeWaitToPressBtn < 1 && stateManager.IsBoxPutDown && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("PressButton") 
                                                     && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !stateManager.MyAnimator.IsInTransition(0))
        {
            stateManager.MyAnimator.SetBool(stateManager.IsPressingButtonAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
            // stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
            stateManager.TargetTrans = stateManager.InfrontDoorTrans;
        }
        //Get out of search box state to Loop come out state
        else if (stateManager.TargetTrans == stateManager.InfrontDoorTrans && Vector3.Distance(stateManager.transform.position, stateManager.InfrontDoorTrans.position) <= 0.01f)
        {
            stateManager.TargetTrans = stateManager.InsideDoorTrans;
            stateManager.SwitchState(stateManager.ComeOutState);
        }
    }
}
