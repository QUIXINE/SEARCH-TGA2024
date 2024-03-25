using System;
using UnityEngine;

public class ClownFlashlightState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        Debug.Log("Enter ClownFlashlightState");
        stateManager.TimeWaitToChangePoint = 0;
        stateManager.TargetTrans = stateManager.InfrontDoorTrans;
        stateManager.Flashlight.SetActive(true);
        stateManager.ViewNotInFlashLight.enabled = false;
        stateManager.ViewInFlashLight.enabled = true;
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        if (stateManager.TargetTrans != null && Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 0.1f
            && stateManager.TargetTrans != stateManager.IsPassedTrans03)
        {
            Debug.Log("Move in FlashLightState");
            if (!stateManager.MyAnimator.GetBool(stateManager.IsWalkingWithFlashlightAnimId.ToString()))
            {
                stateManager.MyAnimator.SetBool(stateManager.IsLookingAroundAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithFlashlightAnimId, true);
            }
            //Move to specific position
            stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
                new Vector3(stateManager.TargetTrans.position.x, stateManager.transform.position.y, stateManager.TargetTrans.position.z), stateManager.WalkSpeed * Time.deltaTime);
            //rotate direction
            if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 1f)
            {
                Debug.Log("Rotate in FlashLightState");
                Vector3 dir = stateManager.TargetTrans.position - stateManager.transform.position;
                dir.z *= -1;
                dir.x *= -1;
                Vector3 rotation = Vector3.RotateTowards(stateManager.transform.forward, dir, stateManager.RotationSpeed * Time.deltaTime, 0f);
                stateManager.transform.rotation = Quaternion.LookRotation(rotation);
            }
            else
            {
                // Debug.Log("Another Rotate in FlashLightState");
                // stateManager.transform.rotation = Quaternion.Euler(0,-60,0);
                // stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, Quaternion.Euler(0,-60,0), stateManager.RotationSpeed * Time.deltaTime);
                stateManager.MyRigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
            }
        }
        else if ((Vector3.Distance(stateManager.transform.position, stateManager.IsPassedTrans01.position) < 0.1f || 
                  Vector3.Distance(stateManager.transform.position, stateManager.IsPassedTrans02.position) >= 0.1f) && 
                 stateManager.transform.rotation != Quaternion.Euler(0,-60,0))
        {
            Debug.Log("Else if Rotate in FlashLightState");
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, Quaternion.Euler(0,-60,0), stateManager.RotationSpeed * Time.deltaTime);
        }
            
        else
        {
            stateManager.MyRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        }
        
        
        if (stateManager.TargetTrans != null && Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 0.1f
                && stateManager.TargetTrans == stateManager.IsPassedTrans03)
        {
            //Assign animation
            if (stateManager.MyAnimator.GetBool(stateManager.IsWalkingWithFlashlightAnimId) && !stateManager.MyAnimator.GetBool(stateManager.IsLookingAroundAnimId))
            {
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithFlashlightAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsLookingAroundAnimId, true);
            }
            
            //after get to final line for ... sec., move out of the room then deactivate the clown
            if (stateManager.TimeWaitToChangePoint > 0)
            {
                Debug.Log("Decrease time");
                stateManager.TimeWaitToChangePoint -= Time.deltaTime;
            }
            else if (stateManager.TimeWaitToChangePoint <= 0)
            {
                Debug.Log("Move in FlashLightState IsPassedTrans03");
                
                //Assign animation
                if (!stateManager.MyAnimator.GetBool(stateManager.IsWalkingWithFlashlightAnimId) && stateManager.MyAnimator.GetBool(stateManager.IsLookingAroundAnimId))
                {
                    stateManager.MyAnimator.SetBool(stateManager.IsLookingAroundAnimId, false);
                    stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithFlashlightAnimId, true);
                }
                
                //Move to specific position
                stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position,
                    new Vector3(stateManager.TargetTrans.position.x, stateManager.transform.position.y, stateManager.TargetTrans.position.z), stateManager.WalkSpeed * Time.deltaTime);
                //rotate direction
                if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 1f)
                {
                    Debug.Log("Rotate in FlashLightState");
                    Vector3 dir = stateManager.transform.position - stateManager.TargetTrans.position;
                    Vector3 rotation = Vector3.RotateTowards(stateManager.transform.forward, dir, stateManager.RotationSpeed * Time.deltaTime, 0f);
                    stateManager.transform.rotation = Quaternion.LookRotation(rotation);
                }
            }
        }
        else if (stateManager.TargetTrans != null && Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) < 0.1f && stateManager.TargetTrans == stateManager.IsPassedTrans03)
        {
            stateManager.TargetTrans = stateManager.IsPassedTrans04;
        }
        else if (stateManager.TargetTrans != null && Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) < 0.1f && stateManager.TargetTrans == stateManager.IsPassedTrans04)
        {
            stateManager.gameObject.SetActive(false);
        }

        
        //Assign target transform
        if (Vector3.Distance(stateManager.transform.position, stateManager.InfrontDoorTrans.position) <= 0.2f)
        {
            Debug.Log("go ot btn pos");
            stateManager.TargetTrans = stateManager.ButtonTrans;
        }
        else if (stateManager.IsComeToPos)
        {
            stateManager.TargetTrans = null;
            stateManager.transform.position = stateManager.ComeToPosTrans.position;
            stateManager.IsComeToPos = false;
        }
        else if (stateManager.IsPassed01)
        {
            stateManager.TargetTrans = stateManager.IsPassedTrans01;
            stateManager.IsPassed01 = false;
        }
        else if (stateManager.IsPassed02)
        {
            stateManager.TargetTrans = stateManager.IsPassedTrans02;
            stateManager.IsPassed02 = false;
        }
        /*else if (stateManager.IsPassed03)
        {
            stateManager.TargetTrans = stateManager.IsPassedTrans03;
            stateManager.IsPassed03 = false;
        }*/
        //check distance to assign time and next target pos
        else if (stateManager.TargetTrans == stateManager.IsPassedTrans02 && Vector3.Distance(stateManager.transform.position, stateManager.IsPassedTrans02.position) <= 0.2f
                && stateManager.TimeWaitToChangePoint <= 0)
        {
                stateManager.TimeWaitToChangePoint = 15f;
                stateManager.TargetTrans = stateManager.IsPassedTrans03;
        }

        
        //Assign Animation
        //When target pos is IsPassedTrans01 and IsPassedTrans03 (IsPassedTrans03 because inside **check distance to assign time and next target pos** changes target pos to IsPassedTrans03
        //which means when reach IsPassedTrans02 the target pos is already changed to IsPassedTrans03)
        
        //can't use while the clown is at IsPassedTrans02 because this condition also checks distance between target pos and current pos 
        if (stateManager.TargetTrans != null && Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) < 0.1f
            && stateManager.TargetTrans != stateManager.IsPassedTrans04)
        {
            stateManager.MyAnimator.SetBool(stateManager.IsWalkingWithFlashlightAnimId, false);
            stateManager.MyAnimator.SetBool(stateManager.IsLookingAroundAnimId, true);
            Debug.Log("Assign LookAround Animation: " + stateManager.MyAnimator.GetBool(stateManager.IsLookingAroundAnimId)
            + "Assign Walking Animation: " + stateManager.MyAnimator.GetBool(stateManager.IsWalkingWithFlashlightAnimId));
        }
        
        
    }
}
