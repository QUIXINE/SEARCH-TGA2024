using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class ClownStateManager : MonoBehaviour
{
    public ClownIdleState IdleState = new ClownIdleState();
    public ClownComeOutState ComeOutState = new ClownComeOutState();
    public ClownSearchBoxState SearchBoxState = new ClownSearchBoxState();
    public ClownChaseState ChaseState = new ClownChaseState();
    public ClownAttackState AttackState = new ClownAttackState();
    public ClownFlashlightState FlashlightState = new ClownFlashlightState();
    public ClownBaseState CurrentState { get; private set; }

    [Header("Animation")]
    public Animator MyAnimator;
    public int IsPuttingDownAnimId = Animator.StringToHash("IsPuttingDown");
    public int IsIdleAnimId = Animator.StringToHash("IsIdle");
    public int IsWalkingAnimId = Animator.StringToHash("IsWalking");
    public int IsWalkingWithCrateAnimId = Animator.StringToHash("IsWalkingWithCrate");
    public int IsWalkingWithFlashlightAnimId = Animator.StringToHash("IsWalkingWithFlashlight");
    public int IsPressingButtonAnimId = Animator.StringToHash("IsPressingButton");
    public int IsLookingAroundAnimId = Animator.StringToHash("IsLookingAround");
    
    [Header("General")]
    public float TimeWaitToChangePoint;
    public float TimeToStayAtTable;
    public float TimeWaitToComeOutAfterBoxIsPutDown;
    public float ChaseRange;
    public float AttackRange;
    public float WalkSpeed;
    public float ChaseSpeed;
    public float RotationSpeed;
    [FormerlySerializedAs("ClownRigidbody")] public Rigidbody MyRigidbody;
    public FieldOfView ViewInFlashLight;
    public FieldOfView ViewNotInFlashLight;
    public Transform RightHandTrans;
    public GameObject Flashlight;
    
    //Transform (position to move to)
    [Header("Waypoints")]
    public Transform PlayerTrans;
    public Transform InsideDoorTrans;
    public Transform InfrontDoorTrans;
    public Transform TableTrans;
    public Transform ButtonTrans;
    public Transform InsideSecretDoorTrans;
    public Transform BoxTrans;
    public Transform TargetTrans;
    
    
    //Come out state vars
    [Header("Come Out State Variables")]
    public bool IsReadyToComeOut;
    public bool IsReadyToComeOutGetBox;
    public float TimeWaitToChangeState;
    [Tooltip("Used to check whether if the table position is the next target. if the clown is at InfrontDoorTrans he has to decide which way to go (inside door or table), " +
             "this bool will help checking the decision to loop from table and inside door position after the box is delivered")]
    public bool IsTableNextTarget;
    
    //Search Box State
    [Header("Search Box State Variables")]
    public SecretDoorButton DoorButton;
    public Transform HoldTrans;
    public float TimeWaitToPressBtn;
    public float RayToBoxDistance;
    public bool IsBoxPutDown;
    
    //Flashlight State
    public Transform ComeToPosTrans, IsPassedTrans01, IsPassedTrans02, IsPassedTrans03, IsPassedTrans04;
    public bool IsComeToPos, IsPassed01, IsPassed02, IsPassed03;
    
    private void Start()
    {
        PlayerTrans = FindObjectOfType<PlayerController>().transform;
        MyRigidbody = GetComponent<Rigidbody>();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update() 
    {
        CurrentState.UpdateState(this);
    }
    
    public void SwitchState(ClownBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }
}
