   using UnityEngine;

   public class WendigoChaseState : WendigoBaseState
    {
        public override void EnterState(WendigoStateManager stateManager)
        {
            stateManager.MyAnimator.SetBool(stateManager.WalkAnimId, true);
        }

        public override void UpdateState(WendigoStateManager stateManager)
        {
            Chase(stateManager);
            ChangeState(stateManager);
        }

        private static void Chase(WendigoStateManager stateManager)
        {
            //Normal and aggressive chase
            bool isCloseForAggressiveChase = Vector3.Distance(stateManager.transform.position, stateManager.PlayerTrans.position) <= stateManager.ChaseAggressivelyRange;
            // Debug.Log("Distance of chase: " + Vector3.Distance(stateManager.transform.position, stateManager.PlayerTrans.position));
            stateManager.CC.Move(new Vector3(1 * (isCloseForAggressiveChase? stateManager.ChaseAggressivelySpeed : stateManager.ChaseNormalSpeed) * Time.deltaTime, -9.8f,
                0));
            if (isCloseForAggressiveChase)
            {
                Debug.Log("Increase Anim Speed");
                stateManager.MyAnimator.SetFloat("ChaseSpeed", stateManager.ChaseAnimSpeedControl);
            }
        }
        
        private static void ChangeState(WendigoStateManager stateManager)
        {
            if (Vector3.Distance(stateManager.transform.position, stateManager.PlayerTrans.position) <= stateManager.AttackRange)
            {
                stateManager.SwitchState(stateManager.AttackState);
            }
        }

    }
