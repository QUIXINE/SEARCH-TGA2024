using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public float HorizontalInput;
    private Rigidbody _rb;
    private Animator _animator;
    [SerializeField] private float _speed;
    private string _sceneName;
    
    
    // Jump
    [SerializeField] private float _jumpForce;
    private bool _isJumpPressed;
    
    // Make these properties because they are used to adjust the speed of player.
    // Why adjust --> at first Move() and Jump(_) is in Update() which makes player moves weirdly, so I make trigger to adjust player speed
    // in hope that it will help make player move and jump naturally
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            if (value <= 1500)
            {
                _speed = value;
            }
        }
    }
    public float JumpForce
    {
        get
        {
            return _jumpForce;
        }
        set
        {
            if (value <= 1500)
            {
                _jumpForce = value;
            }
        }
    }
    
    [Header("Rotate Player")]
    [SerializeField] private float _interpolatioRatio;

    #region Move Object Check

        [Header("Move Object Check")] 
        public bool IsOnMovableItem;
        [SerializeField] private LayerMask _movableObjLayerMask;
        [SerializeField] private Transform _movableObjDetectorLeft;
        [SerializeField] private Transform _movableObjDetectorRight;
        [SerializeField] private Transform _movableObjDetectorCenter;
        [SerializeField] private Transform _movableObjParent;
        [SerializeField] private float _detectorRayDistance;
        [SerializeField] private float _detectorRollRayDistance;
        private GameObject _grabbedObj;
        private bool _isNearMovableObject;
        private bool _isPulling;
        private bool _isPushingBox;
        private bool _isPushingLog;
        private Vector3 _playerYPos;
        private float _horizontalCheck;
        
    #endregion

    #region Slide Check

        //Slide Check
        [Header("Slide Check")] 
        public bool IsSlideFound;
        [SerializeField] private float _xJumpForce;
        [SerializeField] private float _jumpForceOnSlide;
        private Vector3 _slopeSlideVelocity;
        private bool _isJumpOnSlidePressed;
        /* old ones
            [SerializeField] private Transform _slidingDestinationPoint;
            [SerializeField] private float _slidingSpeedX;
            [SerializeField] private float _slidingSpeedY;
            [SerializeField] private float _slopeLimit;
            [SerializeField] private float _ySpeed;
            [SerializeField] private float _slideSpeed;
            private bool _isSlideOff;
            private bool _isSliding;
        */

    #endregion

        
    #region Ledge Climb

        [Header("Ledge Climb")]
        
        [Header("Ledge Check")]
        [SerializeField] private float _lineDownSpace;
        [SerializeField] private float _lineDownStartLength;
        [SerializeField] private float _lineDownEndLength;
        [SerializeField] private float _lineFwdStartLength;
        [SerializeField] private float _lineFwdEndLength;
        [Tooltip("hang offset between player and ledge on z-axis")]
        [SerializeField] private float _hangOffsetMulFwd;
        [Tooltip("hang offset between player and ledge on y-axis")]
        [SerializeField] private float _hangOffsetMulUp;
        private bool _hanging;
        private bool _isClimbingLedge;
        private bool _isReadyToHang;
        bool _isReadyToMoveToX;
        
        [Header("Ledge Climbing")]
        [SerializeField] private float _climbLedgeSpeed;
        [FormerlySerializedAs("_topPosYAddValue")] [SerializeField] private float _LedgeTopPosYAddValue;
        private Vector3 _ledgeTopPos;
        private Vector3 _xPosToClimbLedgeTo;
        [FormerlySerializedAs("_xPosToClimbToValue")]
        [Tooltip("x-axis value for player to climb to after getting off the ledge")]
        [SerializeField] private float _xPosToClimbLedgeToValue;
        [FormerlySerializedAs("_xPosToClimbToLerpSpeed")] [SerializeField] private float _xPosToClimbLedgeToLerpSpeed;
        [FormerlySerializedAs("_xPosToClimbLedgeToLerpFromSpeed")] [FormerlySerializedAs("_xPosToClimbToLerpFromSpeed")] [SerializeField] private float _xPosToClimbLadderToLerpFromSpeed;
        

    #endregion
    
    [Header("Ground Check")]
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private float _groundDetectorRayDistance;
    [SerializeField] private LayerMask _groundLayerMask;
    public bool IsOnGround { get; private set; }

    #region Ladder Climb Variables

        [Header("Ladder Climb")]
        [SerializeField] private float _climbLadderSpeed;
        [SerializeField] private float _getOffLadderSpeed;
        [SerializeField] private float _ladderCheckRayDistance;
        [SerializeField] private Transform _ladderDetector;
        [SerializeField] private float _heightFromLadderCheck;
        [SerializeField] private LayerMask _transportLayerMask;
        [SerializeField] private float _ladderTopPosYAddValue;
        private Vector3 _xPosToClimbLadderTo;
        [FormerlySerializedAs("_xPosToClimbToValue")]
        [Tooltip("x-axis value for player to climb to after getting off the ledge")]
        [SerializeField] private float _xPosToClimbLadderToValue;
        [SerializeField] private float _yPosToClimbLadderToValue;
        [SerializeField] private float _xPosToClimbLadderToLerpSpeed;
        [SerializeField] private float _xPosToClimbladderToLerpFromSpeed;
        [SerializeField] private float _xOffsetClimbDownLadderValue;
        [SerializeField] private float _yOffsetClimbDownLadderValue;
        private bool _isLadderFound;
        private bool _isGettingOffLadder;
        private bool _isAtTopOfLadder;
        private bool _isReadyToClimbLadderDown;
        private bool _isGoingUpLadder;
        private bool _isGoingDownLadderX;
        private bool _isGoingDownLadderY;
        private float _vertical;
        private float _yRotationValue;
        private Vector3 _ladderTopPosClimbUp;
        private Vector3 _ladderTopPosClimbDownX;
        private Vector3 _ladderTopPosClimbDownY;

    #endregion
    
    private readonly int _moveSpeedAnimId = Animator.StringToHash("MoveSpeed");
    private readonly int _isWalkingAnimId = Animator.StringToHash("IsWalking");
    private readonly int _isJumpingAnimId = Animator.StringToHash("IsJumping");
    private readonly int _isFallingAnimId = Animator.StringToHash("IsFalling");
    private readonly int _isClimbingLadderAnimId = Animator.StringToHash("IsClimbingLadder");
    private readonly int _climbLadderSpeedAnimId = Animator.StringToHash("ClimbLadderSpeed");
    private readonly int _isClimbingLedgeAnimId = Animator.StringToHash("IsClimbingLedge");
    private readonly int _isPushingBoxAnimId = Animator.StringToHash("IsPushingBox");
    private readonly int _isPushingLogAnimId = Animator.StringToHash("IsPushingLog");
    private readonly int _isPullingBoxAnimId = Animator.StringToHash("IsPullingBox");
    private readonly int _isPullingDollAnimId = Animator.StringToHash("IsPullingDoll");

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerYPos = new Vector3(0, transform.position.y, 0);
        _sceneName = gameObject.scene.name;
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        LedgeCheck();
        ClimbLadder();
    }

    void Update()
    {
        MoveInput();
        PushAndPull();
        GroundCheck();
        RollRock();
        JumpInput();
        LedgeClimbInput();
        ClimbLadderInput();
    }

    private void MoveInput()
    {
        if (!IsSlideFound)
        {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            HorizontalInput = 0;
        }
    }

    private void Move()
    {
        if ((HorizontalInput > 0 || HorizontalInput < 0) && IsOnGround && !_hanging)
        {
            Quaternion dir;
            if (!_isPulling)
            {
                if (HorizontalInput > 0)
                {
                    dir = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    dir = Quaternion.Euler(0, -90, 0);
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, dir, _interpolatioRatio);
            }
            
            // if not pushing then assign walking animation
            if (!_animator.GetBool(_isPushingBoxAnimId) && !_animator.GetBool(_isPullingBoxAnimId) && !_isPulling 
                && !_isPushingBox && !_isPushingLog && !_isNearMovableObject)
            {
                _animator.SetBool(_isWalkingAnimId, true);
                _animator.SetFloat(_moveSpeedAnimId, HorizontalInput);
                if (HorizontalInput != 0)
                {
                    print("Play");
                    if (_sceneName == "PlayerForest")
                    {
                        AudioManager.Instance.PlaySFX("FFootStep", .05f);
                    }
                    else if (_sceneName == "PlayerClownHouse")
                    {
                        AudioManager.Instance.PlaySFX("CHFootStep", .05f);
                    }
                }
                else
                {
                    AudioManager.Instance.StopSFXInObject(AudioManager.Instance.sfxSource);
                }
                
            }
            else
            {
                _animator.SetBool(_isWalkingAnimId, false);
                _animator.SetFloat(_moveSpeedAnimId, 0);
            }
            
            //if use MovePosition player won't do the curve jump
            //if use velocity player can do the curve jump but the value of _speed has to be set so high
            _rb.velocity = new Vector3((HorizontalInput * _speed) * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);
            // Vector3 xPos = new Vector3(_horizontalInput, _rb.velocity.y, 0);
            // _rb.MovePosition(transform.position + xPos * (_speed * Time.deltaTime));

        }
        else
        {
            _animator.SetBool(_isWalkingAnimId, false);
            _animator.SetFloat(_moveSpeedAnimId, 0);
        }
    }
    
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsOnGround && !IsSlideFound)
        {
            _isJumpPressed = true;
            _animator.SetBool(_isJumpingAnimId, true);
            _animator.SetBool(_isWalkingAnimId, false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && IsOnGround && IsSlideFound)
        {
            _isJumpOnSlidePressed = true;
            _animator.SetBool(_isJumpingAnimId, true);
            _animator.SetBool(_isWalkingAnimId, false);
        }
    }

    private void Jump()
    {
        // used !_hanging to prevent jump while hanging
        if (_isJumpPressed && !_hanging)
        {
            print("Jump");
            /*if (_hanging /*&& Mathf.Approximately(_rb.velocity.y, 0)#1#)
            {
                print("jump + _hanging");
                //_animator.SetBool(IsJumpingAnimId, true);
                _rb.useGravity = true;
                // _rb.isKinematic = false;
                _hanging = false;
                _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {*/
            _rb.velocity = new Vector3(0, 0, 0);
            _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
            // }
            // _animator.SetBool(IsJumpingAnimId, false);
            _isReadyToHang = true;
            _isJumpPressed = false;
        }
        /*else if (_isJumpOnSlidePressed)
        {
            print("Jump On Slide");
            _rb.AddForce(new Vector3(_xJumpForce, 1 * _jumpForceOnSlide, 0) * Time.deltaTime, ForceMode.Impulse);
            _isJumpOnSlidePressed = false;

        }*/
        else if (!_isJumpPressed && IsOnGround && _rb.velocity.y <= -1f && !_animator.GetBool(_isWalkingAnimId))
        {
            _isReadyToHang = false;
            _animator.SetBool(_isFallingAnimId, true);
            _animator.SetBool(_isJumpingAnimId, false);
        }
        else if (IsOnGround && _animator.GetBool(_isFallingAnimId))
        {
            _isReadyToHang = false;
            _animator.SetBool(_isFallingAnimId, false);
        }
        else if (IsOnGround && _rb.velocity.y == 0f && _animator.GetBool(_isJumpingAnimId))
        {
            _isReadyToHang = false;
            _animator.SetBool(_isJumpingAnimId, false);
        }
        
    }

    private void PushAndPull()
    {
        RaycastHit hit;
        _isNearMovableObject = /*Physics.Raycast(_movableObjDetectorLeft.position, Vector3.left, out hit,_detectorRayDistance) || Physics.Raycast(_movableObjDetectorLeft.position, Vector3.right, out hit,_detectorRayDistance)
                               || */Physics.Raycast(_movableObjDetectorRight.position, Vector3.right, out hit,_detectorRayDistance, _movableObjLayerMask)
                               || Physics.Raycast(_movableObjDetectorRight.position, Vector3.left, out hit,_detectorRayDistance, _movableObjLayerMask);
        
        if (!_isPulling && _grabbedObj == null &&_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.RightControl))
        {
            print("Grab");
            _rb.constraints |= RigidbodyConstraints.FreezePositionY;
            //hit.collider.transform.parent.parent = _movableObjParent; --> original, has no if-else under
            if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) 
                     && hit.collider.gameObject.TryGetComponent<MovableObjectCheckMoving>(out MovableObjectCheckMoving movableObj))
            {
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                rb.isKinematic = true;
                movableObj.IsMoving = false;
                HandleRotation();
                hit.collider.transform.parent = _movableObjParent;
            }
            else if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb2))
            {
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                rb2.isKinematic = true;
                _grabbedObj = hit.collider.gameObject;
                HandleRotation();
                hit.collider.transform.parent = _movableObjParent;
            }
            else if(hit.collider.transform.parent) //use w/ bear trap or other objs that have parent
            {
                HandleRotation();
                hit.collider.transform.parent.parent = _movableObjParent;
            }
            else
            {
                HandleRotation();
                hit.collider.transform.parent = _movableObjParent;
            }

            
            // This condition problem is it makes !isPulling raotate condition in Move() works which makes _movableObj rotates along we/ player 
            // if player is less than box's position and horizontal < 0 --> pull
            // if player is greater than box's position and horizontal < 0 --> push
            /*if ((transform.position.x > hit.collider.transform.localPosition.x && HorizontalInput > 0)
                || (transform.position.x < hit.collider.transform.localPosition.x && HorizontalInput < 0))
            {
                _isPulling = true;
                HandleRotation();
            }*/
            _isPulling = true;
            _horizontalCheck = HorizontalInput;
            HandleAnimationPull();
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;

        }
        else if (!_isPulling && _grabbedObj == null && _isNearMovableObject &&
                 hit.collider.CompareTag(TagManager.Branch) && Input.GetKey(KeyCode.RightControl))
        {
            print("detects log");
            if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                print("grab log");
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                rb.isKinematic = true;
                _grabbedObj = hit.collider.gameObject;
                HandleRotation();
                hit.collider.transform.parent = _movableObjParent;
                _isPulling = true;
                _horizontalCheck = HorizontalInput;
                HandleAnimationPull();
            }
        }
        
        // using float to get value from Horizontal in condition above and check in this condition to check if the input changes
        // if so change _isGrabbing to false to change push (both animation and bool _isPushing)
        else if((Input.GetKeyUp(KeyCode.RightControl) || _horizontalCheck != HorizontalInput) && _isPulling)
        {
            if (_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) 
                && hit.collider.gameObject.TryGetComponent<MovableObjectCheckMoving>(out MovableObjectCheckMoving movableObj))
            {
                print("release box");
                movableObj.IsMoving = true;
                rb.isKinematic = false;
            }
            else if (_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb2))
            {
                print("release box with Rigidbody");
                _grabbedObj = null;
                rb2.isKinematic = false;
            }
            else if (_isNearMovableObject && hit.collider.CompareTag(TagManager.Branch) && hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb3))
            {
                print("release log");
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                rb3.isKinematic = false;
                rb3.isKinematic = false;
            }
            else if (_grabbedObj != null && _grabbedObj.TryGetComponent<Rigidbody>(out Rigidbody rb4) && !_grabbedObj.TryGetComponent<RockController>(out RockController rockController2))
            {
                rb4.isKinematic = false;
                _grabbedObj = null;
            }
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _movableObjParent.DetachChildren();     //Detach all children inside this parent
            // HandleRotation();
            _isPulling = false;
            HandleAnimationPull();
        }

        if (!IsOnGround)
        {
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        /*else
        {
            _movableObjParent.DetachChildren();     //Detach all children inside this parent
            _isGrabbing = false;
            print("release 2");
        }*/
        
        
        HandleAnimationPush();
        
        void HandleRotation()
        {
            Quaternion dir;
            if (HorizontalInput > 0 && transform.position.x > hit.collider.transform.localPosition.x)
            {
                print("Rotate > 0");
                dir = Quaternion.Euler(0, -90, 0);
                transform.rotation = dir;
            }
            else if (HorizontalInput < 0 && transform.position.x < hit.collider.transform.localPosition.x)
            {
                print("Rotate < 0");
                dir = Quaternion.Euler(0, 90, 0);
                transform.rotation = dir;
            }
        }

        
        void HandleAnimationPush()
        {
            if (HorizontalInput != 0 && _isPushingBox && !_isPulling)
            {
                _animator.SetBool(_isPushingBoxAnimId, _isPushingBox);
            }
            else
            {
                // set here the _isPushing will be false and will only be true if collide w/ movable item's collider
                // which makes _isPushing stays false after release arrow input and stays in collider
                // so I have to make it be true even player stays in the collider
                _animator.SetBool(_isPushingBoxAnimId, false);
            }

            /*if (HorizontalInput != 0 && _isPushingLog && !_isPulling)
            {
                _animator.SetBool(_isPushingLogAnimId, _isPushingLog);
            }
            else
            {
                _animator.SetBool(_isPushingLogAnimId, false);
            }*/
        }

        void HandleAnimationPull()
        {
            if (_isPulling && hit.collider.gameObject.name == "Box")
            {
                _animator.SetBool(_isPullingBoxAnimId, true);
            }
            else if (_isPulling && hit.collider.gameObject.name == "Doll")
            {
                _animator.SetBool(_isPullingDollAnimId, true);
            }
            else
            {
                _animator.SetBool(_isPullingBoxAnimId, false);
                _animator.SetBool(_isPullingDollAnimId, false);
            }
        }
    }

    // try putting each condition inside PushAndPull instead
    void RollRock()
    {
        RaycastHit hit;
        bool IsRockFound =
            Physics.Raycast(_movableObjDetectorCenter.position, Vector3.forward, out hit, _detectorRollRayDistance, _movableObjLayerMask) ||
            Physics.Raycast(_movableObjDetectorCenter.position, Vector3.back, out hit, _detectorRollRayDistance, _movableObjLayerMask);
        if (!_isPulling && IsRockFound && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.RightControl))
        {
            if (hit.collider.gameObject.TryGetComponent<RockController>(out RockController rockController))
            {
                _grabbedObj = hit.collider.gameObject;
                rockController.IsRolled = true;
                _isPulling = true;
            }
            
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;

        }
        else if(Input.GetKeyUp(KeyCode.RightControl) || !IsRockFound)
        {
            if (_grabbedObj && _grabbedObj.gameObject.TryGetComponent<RockController>(out RockController rockController))
            {
                rockController.IsRolled = false;
                _grabbedObj = null;
                _movableObjParent.DetachChildren();     //Detach all children inside this parent
                _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                _isPulling = false;
            }
        }

        //Reposition rock to hit.point so that the rock will be at player's hand
        if (IsRockFound && _grabbedObj != null && hit.collider.gameObject.TryGetComponent<RockController>(out RockController rockController2))
        {
            if (rockController2.IsRolled)
            {
                _grabbedObj.transform.position = new Vector3(hit.point.x, _grabbedObj.transform.position.y, _grabbedObj.transform.position.z);
            }
        }

    }

    /* Old Slides
    private void SlideDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(_groundDetector.position, Vector3.down, out hit, _groundDetectorRayDistance))
        {
            if (hit.collider.gameObject.name == "Slider" && !_isSlideOff)
            {
                print("hit");
                if (HorizontalInput <= 0)
                {
                    HorizontalInput = 1;
                }

                _isSliding = true;

                //_rb.velocity = new Vector3(/*_rb.velocity.x + #1#0, /*_rb.velocity.y + #1#_slidingSpeedY, _slidingSpeedX);
            }
            
        }
        

        if (_isSliding && !_isSlideOff && !_isJumpPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _slidingDestinationPoint.position, _slideSpeed * Time.deltaTime);
        }
        
        /*if (_isSlideOff)
        {
            print("Slide off");
            // _rb.velocity = Vector3.zero;
            if (_horizontalInput <= 0)
            {
                _horizontalInput = 1;
            }
            // _rb.velocity = new Vector3(_rb.velocity.x + (_slidingSpeedX * 2), 0, 0);
            
            transform.Translate(0, _slidingSpeedY, _slidingSpeedX);
            _isSlideOff = false;
            
        }#1#
    }
    
    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, _groundLayerMask))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= _slopeLimit)
            {
                _slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, _ySpeed, 0), hitInfo.normal);
                return;
            }
        }
        
        if (_slopeSlideVelocity != Vector3.zero)
        {
            _isSliding = true;
        }
    }
    */

    #region Ledge Climb
        private void LedgeClimbInput()
        {
            //Press UpArrow to climb up
            if (Input.GetKeyDown(KeyCode.UpArrow) && _hanging)
            {
                print("jump + _hanging");
                _rb.useGravity = true;
                
                // used to prevent jump while hanging
                _isJumpPressed = false;
                
                _isClimbingLedge = true;
            }

            if (_isClimbingLedge)
            {
                LedgeClimb();
            }
        }
        
        private void LedgeCheck()
        {
            //use vertical input or jump to make _hanging false so that when player moves up/down or jump
            //they will get out off the ledge
            if(_rb.velocity.y < 0 && !_hanging && _isReadyToHang)
            {
                print("LedgeCheck()");
                RaycastHit downHit;
                Vector3 lineDownStart = (transform.position + Vector3.up * _lineDownStartLength) + transform.forward * _lineDownSpace;
                Vector3 lineDownEnd = (transform.position + Vector3.up * _lineDownEndLength) + transform.forward * _lineDownSpace;
                Debug.DrawLine(lineDownStart, lineDownEnd);
                Physics.Linecast(lineDownStart, lineDownEnd, out downHit);
                if (downHit.collider != null && (downHit.collider.gameObject.CompareTag(TagManager.Ledge) || downHit.collider.gameObject.CompareTag(TagManager.Branch)
                        || downHit.collider.gameObject.CompareTag(TagManager.Lift) || downHit.collider.gameObject.CompareTag(TagManager.MovableItem)))
                {
                    RaycastHit fwdHit;
                    Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - _lineFwdStartLength, transform.position.z);
                    Vector3 lineFwdEnd = new Vector3(transform.position.x + (transform.rotation.y > 0? 5 : -5), downHit.point.y - _lineFwdEndLength, transform.position.z)/* +
                                         transform.forward*/;
                    Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit);
                    Debug.DrawLine(lineFwdStart, lineFwdEnd);

                    if (fwdHit.collider != null && (fwdHit.collider.gameObject.CompareTag(TagManager.Ledge) || fwdHit.collider.gameObject.CompareTag(TagManager.Branch)
                            || fwdHit.collider.gameObject.CompareTag(TagManager.Lift) || fwdHit.collider.gameObject.CompareTag(TagManager.MovableItem)))
                    {
                        _rb.useGravity = false;
                        _rb.velocity = Vector3.zero;
                        _hanging = true;
                        //play animation

                        Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                        Vector3 offset = (transform.forward * _hangOffsetMulFwd) + (transform.up * _hangOffsetMulUp);
                        hangPos += offset;
                        transform.position = hangPos;
                        transform.forward =
                            -fwdHit.normal; //-fwdHit.normal make player face obj by checking the direction of the obj then make it negative
                        //Assign position of the surface above the edge
                        _ledgeTopPos = new Vector3(transform.position.x, fwdHit.collider.bounds.max.y, transform.position.z);
                        _isReadyToHang = false;

                        _animator.SetBool(_isClimbingLedgeAnimId, true);
                        _animator.SetBool(_isJumpingAnimId, false);
                        
                    }
                }

            }
        }

        private void LedgeClimb()
        {
            _rb.useGravity = false;
            // assigned desired x-axis
            if (transform.position.y >= _ledgeTopPos.y + _LedgeTopPosYAddValue -.1f && !_isReadyToMoveToX)
            {
                _xPosToClimbLedgeTo = new Vector3(transform.position.x  + (transform.rotation.y > 0? _xPosToClimbLedgeToValue : -_xPosToClimbLedgeToValue), transform.position.y, transform.position.z);
                _isReadyToMoveToX = true;
            }
            // move up
            else if (transform.position.y < _ledgeTopPos.y + _LedgeTopPosYAddValue + 5f && !_isReadyToMoveToX)
            {
                _animator.SetBool(_isClimbingLedgeAnimId, false);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_ledgeTopPos.x, _ledgeTopPos.y + _LedgeTopPosYAddValue, _ledgeTopPos.z), _climbLedgeSpeed * Time.deltaTime);
            }
            
            // move to desired x-axis
            if (_isReadyToMoveToX && _hanging)
            {
                transform.position = Vector3.MoveTowards(transform.position, _xPosToClimbLedgeTo, Mathf.Lerp(_xPosToClimbLadderToLerpFromSpeed ,_climbLedgeSpeed, _xPosToClimbLedgeToLerpSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, _xPosToClimbLedgeTo) <= 0.1f)
                {
                    _rb.useGravity = true;
                    _isClimbingLedge = false;
                    _isReadyToMoveToX = false;
                    _hanging = false;
                }
            }
        }
    #endregion

    #region Ladder Climb

        private void ClimbLadderInput()
        {
            if (_isLadderFound)
            {
                _vertical = Input.GetAxisRaw("Vertical");
            }
            
            // GetToTop
            // check if at the top of the ladder in OnTrigger -> _isAtTheTopOfLadder
            // if _isAtTheTopOfLadder = true, stop moving and automatic climb to the top like ledge climb
            if ((Input.GetKeyDown(KeyCode.UpArrow) || _vertical > 0) && _isAtTopOfLadder)
            {
                print("Call Coroutine UpArrow");
                _isGoingUpLadder = true;
                StartCoroutine(ClimbUpLadder());
                _isAtTopOfLadder = false;
            }
            
            // GetDown
            // if _isAtTopOfLadder = true and press down arrow make player move to the ladder
            // rotate player to face the ladder if move from the right, rotates player to face left
            // move player to _topPos
            if (Input.GetKeyDown(KeyCode.DownArrow) && _isReadyToClimbLadderDown && !_isLadderFound)
            {
                print("Call Coroutine DownArrow");
                transform.rotation = Quaternion.Euler(transform.rotation.x, _yRotationValue, transform.rotation.z);
                // _isLadderFound = true;
                _isGoingDownLadderX = true;
                StartCoroutine(ClimbDownLadder());
                _isReadyToClimbLadderDown = false;
            }
        }
        
        private bool LadderCheck()
        {
            bool isRayFoundLadder = Physics.Raycast(_ladderDetector.position, transform.forward, out RaycastHit hitInfo, 
                _ladderCheckRayDistance, _transportLayerMask, QueryTriggerInteraction.Ignore);
            // ray cast to check ladder. if find ladder, make player cling onto it, change bool to true
            Debug.DrawLine(_ladderDetector.position, new Vector3(_ladderDetector.position.x + _ladderCheckRayDistance, 
                _ladderDetector.position.y, _ladderDetector.position.z), Color.green);
            if (isRayFoundLadder && !_isLadderFound)
            {
                if (hitInfo.collider.gameObject.CompareTag(TagManager.Ladder))
                {
                    print("Found Ladder!");
                    // make player cling onto ladder
                    //
                    transform.position = new Vector3(hitInfo.point.x,
                        hitInfo.point.y  - _heightFromLadderCheck, hitInfo.point.z);
                    _rb.useGravity = false;
                    //
                    //
                    _ladderTopPosClimbUp = new Vector3(transform.position.x, hitInfo.collider.bounds.max.y, transform.position.z);
                    _animator.SetBool(_isJumpingAnimId, false);
                    if (!_animator.GetBool(_isClimbingLadderAnimId))
                    {
                        _animator.SetBool(_isClimbingLadderAnimId, true);
                    }
                    return true;
                }
                
            }
            return false;
        }

        private void ClimbLadder()
        {
            if (_isGettingOffLadder)
            {
                GetOff();
            }
            else
            {
                Climb();
            }
            
            void Climb()
            {
                print("Climb");
                if (LadderCheck() && !_isLadderFound)
                {
                    _isLadderFound = true;
                }

                if (_isLadderFound && _vertical != 0 && !_isGettingOffLadder)
                {
                    _rb.velocity = new Vector3(0, _vertical * _climbLadderSpeed * Time.deltaTime, 0);
                    _animator.SetFloat(_climbLadderSpeedAnimId, _vertical);
                }
                else if (_isLadderFound && _vertical == 0)
                {
                    _rb.velocity = Vector3.zero;
                    _animator.SetFloat(_climbLadderSpeedAnimId, _vertical);
                }
            }
            
            void GetOff()
            {
                print("Get off the ladder");
                _rb.velocity = new Vector3(0,-1 * _getOffLadderSpeed * Time.deltaTime,0);
                if (_animator.GetBool(_isClimbingLadderAnimId))
                {
                    _animator.SetBool(_isClimbingLadderAnimId, false);
                }
            }
            
            // GetToTop
            // check if at the top of the ladder in OnTrigger -> _isAtTheTopOfLadder
            // if _isAtTheTopOfLadder = true, stop moving and automatic climb to the top like ledge climb
            /*void GetToTop()
            {
                print("GetToTop()");
                transform.position = Vector3.MoveTowards(transform.position, _xPosToClimbLadderTo, Mathf.Lerp(_xPosToClimbLadderToLerpFromSpeed ,_climbLadderSpeed, _xPosToClimbLadderToLerpSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, _xPosToClimbLadderTo) <= 0.1f)
                {
                    _rb.useGravity = true;
                    _isLadderFound = false;
                    _isAtTopOfLadder = false;
                }
            }*/
            
            // GetDown
            // if _isAtTopOfLadder = true and press down arrow make player move to the ladder
            // rotate player to face the ladder if move from the right, rotates player to face left
            // move player to _topPos
            
        }

        private IEnumerator ClimbUpLadder()
        {
            while (_isGoingUpLadder)
            {
                print("Climb Up");
                transform.position = Vector3.MoveTowards(transform.position, _xPosToClimbLadderTo, Mathf.Lerp(_xPosToClimbLadderToLerpFromSpeed ,_climbLadderSpeed, _xPosToClimbLadderToLerpSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, _xPosToClimbLadderTo) <= 0.1f)
                {
                    _rb.useGravity = true;
                    _isLadderFound = false;
                    if (_animator.GetBool(_isClimbingLadderAnimId))
                    {
                        _animator.SetBool(_isClimbingLadderAnimId, false);
                    }
                    _isGoingUpLadder = false;
                }
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        private IEnumerator ClimbDownLadder()
        {
            // make it move to x then down with y
            while (_isGoingDownLadderX)
            {
                print("Climb Down X");
                if (Vector3.Distance(transform.position, _ladderTopPosClimbDownX) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _ladderTopPosClimbDownX,
                            _climbLadderSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(transform.position, _ladderTopPosClimbDownX) <= 0.01f)
                {
                    // _isReadyToClimbLadderDown = false;
                    // _isGoingDownLadder = false;
                    _isGoingDownLadderX = false;
                    _isGoingDownLadderY = true;
                    StartCoroutine(ClimbDownLadderY());
                }
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        private IEnumerator ClimbDownLadderY()
        {
            // make it move to x then down with y
            while (_isGoingDownLadderY)
            {
                print("Climb Down Y");
                transform.position = Vector3.MoveTowards(transform.position, _ladderTopPosClimbDownY,
                    _climbLadderSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _ladderTopPosClimbDownY) <= 0.01f)
                {
                    // _isReadyToClimbLadderDown = false;
                    _isGoingDownLadderY = false;
                }
                yield return new WaitForSecondsRealtime(0.001f);
            }
        }
    
    #endregion

    public bool BeOnMovableItemCheck()
    {
        if (Physics.Raycast(_groundDetector.position, Vector3.down, _groundDetectorRayDistance, _movableObjLayerMask))
        {
            _isPushingBox = false;
            return true;
        }
        return false;
    }
    
    private void GroundCheck()
    {
        if (Physics.Raycast(_groundDetector.position, Vector3.down, _groundDetectorRayDistance, _groundLayerMask))
        {
            IsOnGround = true;
            _isGettingOffLadder = false;
            _isLadderFound = false;
        }
        else
        {
            IsOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.Ladder) && _isLadderFound && col.gameObject.name == "Top Of Ladder")
        {
            print("Collide _isLadderFound");
            _xPosToClimbLadderTo = new Vector3(transform.position.x  + (transform.rotation.y > 0? _xPosToClimbLadderToValue : -_xPosToClimbLadderToValue), 
                _ladderTopPosClimbUp.y + _yPosToClimbLadderToValue, 0);
            _yRotationValue = col.gameObject.GetComponent<RotationValueClimbLadderDown>().YRotationValue;
            _isAtTopOfLadder = true;
            _isReadyToClimbLadderDown = true;
        }
        
        else if (col.gameObject.CompareTag(TagManager.Ladder) && !_isLadderFound && col.gameObject.name == "Top Of Ladder Down")
        {
            print("Collide !_isLadderFound");
            _isReadyToClimbLadderDown = true;
            // get toppos value from parent of trigger. parent is ladder game object
            _ladderTopPosClimbDownX = new Vector3(col.bounds.max.x + (col.gameObject.GetComponent<RotationValueClimbLadderDown>().YRotationValue > 0 ? -_xOffsetClimbDownLadderValue : _xOffsetClimbDownLadderValue), 
                col.bounds.min.y, 0);
            _ladderTopPosClimbDownY = new Vector3(_ladderTopPosClimbDownX.x, col.bounds.min.y - _yOffsetClimbDownLadderValue, 0);
            _yRotationValue = col.gameObject.GetComponent<RotationValueClimbLadderDown>().YRotationValue;
            print("_ladderTopPos "+_ladderTopPosClimbUp);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // This may make velocity always goes down 
        if (col.gameObject.CompareTag(TagManager.Ladder) && _isLadderFound && col.gameObject.name != "Top Of Ladder"  && col.gameObject.name != "Top Of Ladder Down")
        {
            _rb.useGravity = true;
            _isGettingOffLadder = true;
        }
        else if (col.gameObject.CompareTag(TagManager.Ladder) && col.gameObject.name == "Top Of Ladder Down")
        {
            _isReadyToClimbLadderDown = false;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem) && col.gameObject.name == "Box")
        {
            _isPushingBox = true;
            _animator.SetBool(_isPushingBoxAnimId, _isPushingBox);
            // _animator.SetFloat(_moveSpeedAnimId, 0);
        }
        else if (col.gameObject.CompareTag(TagManager.Branch) && !BeOnMovableItemCheck())
        {
            _isPushingLog = true;
            _animator.SetBool(_isPushingLogAnimId, _isPushingLog);
        }
    }
    
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem) && col.gameObject.name == "Box")
        {
            _isPushingBox = false;
            _animator.SetBool(_isPushingBoxAnimId, _isPushingBox);
        }
        else if (col.gameObject.CompareTag(TagManager.Branch))
        {
            _isPushingLog = false;
            _animator.SetBool(_isPushingLogAnimId, _isPushingLog);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_movableObjDetectorCenter.position, new Vector3(_movableObjDetectorCenter.position.x, _movableObjDetectorCenter.position.y, _movableObjDetectorCenter.position.z  + _detectorRollRayDistance));
    }
}
// Problem : isonground is checked eventhough plyer is hangginh above the ground