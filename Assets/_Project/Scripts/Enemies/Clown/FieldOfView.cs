using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    [SerializeField] private ClownStateManager _clownStateManager;
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    if (_clownStateManager.CurrentState != _clownStateManager.AttackState 
                        && _clownStateManager.CurrentState != _clownStateManager.ChaseState)
                    {
                        Debug.Log("damage 1");
                        //_clownStateManager.enabled = false;
                        /*var takeDamage = _clownStateManager.PlayerTrans.gameObject.GetComponent<PlayerTakeDamage>();
                        takeDamage.TakeDamage(gameObject.scene);*/
                        _clownStateManager.SwitchState(_clownStateManager.ChaseState);
                    }
                }
                /*else if (canSeePlayer && Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask) && gameObject.name != "Flashlight")
                {
                    Debug.Log("Switch to Idle State");
                    _clownStateManager.SwitchState(_clownStateManager.IdleState);
                }*/
                else if (canSeePlayer && gameObject.name == "Flashlight")
                {
                    Debug.Log("damage 2");
                    var takeDamage = _clownStateManager.PlayerTrans.gameObject.GetComponent<PlayerTakeDamage>();
                    takeDamage.TakeDamage(gameObject.scene);
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    
    //add field of view for flashlight because flashlight and eye doesn't work the same flashlight shouldn't make clown get back to idle state if player hides while clown is in chase stae
    //instead it should make player take damage, reload scene and respawn
}