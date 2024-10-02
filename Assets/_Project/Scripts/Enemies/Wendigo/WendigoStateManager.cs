using UnityEngine;
using UnityEngine.Serialization;

public class WendigoStateManager : MonoBehaviour
{
    //States
    public WendigoIdleState IdleState = new WendigoIdleState();
    public WendigoComeOutState ComeOutState = new WendigoComeOutState();
    public WendigoChaseState ChaseState = new WendigoChaseState();
    public WendigoAttackState AttackState = new WendigoAttackState();
    private WendigoBaseState _currentState;
    
    [Header("General")]
    public CharacterController CC;
    public Transform PlayerTrans;
    public bool IsReadyToComeOut;
    public float WalkSpeed;
    public float RotationSpeed;
    public float InterpolationRatio; //What's this called instead of InterpolationRatio

    [Header("Animation")]
    public Animator MyAnimator;
    public int WalkAnimId = Animator.StringToHash("Walk");
    public int AttackAnimId = Animator.StringToHash("Attack");
    public float ChaseAnimSpeedControl = 1f;
    
    [Header("Come Out State's Variables")]
    public Transform TargetTrans;
    public Transform ComeOutTrans;

    //Chase
    [Header("Chase State's Variables")]
    public float ChaseNormalSpeed;
    public float ChaseNormalRange;
    public float ChaseAggressivelySpeed;
    public float ChaseAggressivelyRange;
    
    //Attack
    [Header("Attack State's Variables")]
    public Transform HoldingHandTrans; 
    public Transform HeadTrans; 
    public float AttackRange;
    [Tooltip("Used to reposition player's position while being holded. Why use this? -> because player's transform is at foot position" +
             " so player has to be repositioned down a little so that player's central body will be at Wendigo's hand")]
    public float PlayerYAxisHolded; 
    
    private void Start()
    {
        _currentState = IdleState;
        _currentState.EnterState(this);
        PlayerTrans = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void Update() 
    {
        _currentState.UpdateState(this);
    }
    
    public void SwitchState(WendigoBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }
}