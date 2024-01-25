using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
public class ClownStateManager : MonoBehaviour
{
    public ClownComeOutState ComeOutState = new ClownComeOutState();
    public ClownChaseState ChaseState = new ClownChaseState();
    public ClownIdleState IdleState = new ClownIdleState();
    private ClownBaseState _currentState;

    public NavMeshAgent ClownNavMeshAgent;
    //Chase
    public Transform PlayerTransform;
    public float ChaseSpeed;
    
    private void Start()
    {
        ClownNavMeshAgent = GetComponent<NavMeshAgent>();
        _currentState = ComeOutState;
        _currentState.EnterState(this);
    }

    private void Update() 
    {
        _currentState.UpdateState(this);
    }

    /*private void OnTriggerEnter(Collider col) 
    {
        _currentState.OnTriggerEnterState(col, this);
    }

    private void OnTriggerExit(Collider col) 
    {
        _currentState.OnTriggerExitState(col, _arrivedAtDestionation);
    }*/
    
    public void SwitchState(ClownBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }
}
