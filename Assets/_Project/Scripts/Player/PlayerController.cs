using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    [FormerlySerializedAs("_horizontalInput")] public float HorizontalInput;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private bool _isJumpPressed;
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

    [Header("Move Object Check")] 
    [SerializeField] private LayerMask _movableObjLayerMask;
    [SerializeField] private Transform _movableObjDetectorLeft;
    [SerializeField] private Transform _movableObjDetectorRight;
    [SerializeField] private Transform _movableObjDetectorCenter;
    [SerializeField] private Transform _movableObjParent;
    [SerializeField] private float _detectorRayDistance;
    [SerializeField] private float _detectorRollRayDistance;
    private GameObject _grabbedObj;
    private bool _isNearMovableObject;
    private bool _isGrabbing;
    private Vector3 _playerYPos;

    //Slide Check
    [Header("Slide Check")]
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private Transform _slidingDestinationPoint;
    [SerializeField] private float _groundDetectorRayDistance;
    [SerializeField] private float _slidingSpeedX;
    [SerializeField] private float _slidingSpeedY;
    [SerializeField] private float _slopeLimit;
    [SerializeField] private float _ySpeed;
    [SerializeField] private float _slideSpeed;
    private Vector3 _slopeSlideVelocity;
    private bool _isSlideOff;
    private bool _isSliding;
    
    [Header("Ledge Climb")]
    [SerializeField] private float _lineDownSpace;
    [SerializeField] private float _lineDownStartLength;
    [SerializeField] private float _lineDownEndLength;
    [SerializeField] private float _lineFwdStartLength;
    [SerializeField] private float _lineFwdEndLength;
    [SerializeField] private float _offsetNumMulFwd;
    [SerializeField] private float _offsetNumMulUp;
    private bool _hanging;
    
    public bool IsOnGround { get; private set; }
    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayerMask;

    private int MoveSpeedAnimId = Animator.StringToHash("MoveSpeed");
    private int IsJumpingAnimId = Animator.StringToHash("IsJumping");
    private int IsFallingAnimId = Animator.StringToHash("IsFalling");
    private int IsWalkingAnimId = Animator.StringToHash("IsWalking");
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerYPos = new Vector3(0, transform.position.y, 0);
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
    }

    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        PushAndPull();
        SlideDown();
        GroundCheck();
        RollRock();
        JumpInput();
    }


    private void Move()
    {
        if ((HorizontalInput > 0 || HorizontalInput < 0) && IsOnGround)
        {
            Quaternion dir;
            if (HorizontalInput > 0)
            {
                dir = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                dir = Quaternion.Euler(0, -90, 0);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, dir, _interpolatioRatio);
            _animator.SetFloat(MoveSpeedAnimId, HorizontalInput);
            _animator.SetBool(IsWalkingAnimId, true);
            
            //if use MovePosition player won't do the curve jump
            //if use velocity player can do the curve jump but the value of _speed has to be set so high
            _rb.velocity = new Vector3((HorizontalInput * _speed) * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);
            // Vector3 xPos = new Vector3(_horizontalInput, _rb.velocity.y, 0);
            // _rb.MovePosition(transform.position + xPos * (_speed * Time.deltaTime));

        }
        else
        {
            _animator.SetBool(IsWalkingAnimId, false);
            _animator.SetFloat(MoveSpeedAnimId, 0);
        }
    }
    
    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsOnGround)
        {
            _isJumpPressed = true;
            _animator.SetBool(IsJumpingAnimId, true);
            _animator.SetBool(IsWalkingAnimId, false);
        }
    }

    private void Jump()
    {
        if (_isJumpPressed && IsOnGround)
        {
            if (_hanging)
            {
                print("jump + _hanging");
                //_animator.SetBool(IsJumpingAnimId, true);
                _rb.useGravity = true;
                // _rb.isKinematic = false;
                _hanging = false;
                _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                print("jump");
                _rb.velocity = new Vector3(0, 0, 0);
                _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
                LedgeClimb();
            }
            // _animator.SetBool(IsJumpingAnimId, false);
            _isJumpPressed = false;
        }
        else if (!_isJumpPressed && IsOnGround && _rb.velocity.y <= -1f)
        {
            // _isJumpPressed = false;
            // print("falling");
            _animator.SetBool(IsFallingAnimId, true);
            _animator.SetBool(IsJumpingAnimId, false);
        }
        else if (IsOnGround && _animator.GetBool(IsFallingAnimId))
        {
            // print("stop falling");
            _animator.SetBool(IsFallingAnimId, false);
        }
        else if (IsOnGround && _rb.velocity.y == 0f && _animator.GetBool(IsJumpingAnimId))
        {
            _animator.SetBool(IsJumpingAnimId, false);
        }
    }

    private void PushAndPull()
    {
        RaycastHit hit;
        // RaycastHit hit2;
        _isNearMovableObject = Physics.Raycast(_movableObjDetectorLeft.position, Vector3.left, out hit,_detectorRayDistance) || Physics.Raycast(_movableObjDetectorLeft.position, Vector3.right, out hit,_detectorRayDistance)
                               || Physics.Raycast(_movableObjDetectorRight.position, Vector3.right, out hit,_detectorRayDistance)|| Physics.Raycast(_movableObjDetectorRight.position, Vector3.left, out hit,_detectorRayDistance);
        
        if (!_isGrabbing &&_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.LeftControl))
        {
            print("Grab");
            _rb.constraints |= RigidbodyConstraints.FreezePositionY;
            //hit.collider.transform.parent.parent = _movableObjParent; --> original, has no if-else under
            if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) 
                     && hit.collider.gameObject.TryGetComponent<MovableObjectCheckMoving>(out MovableObjectCheckMoving movableObj))
            {
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                print("grab obj with IsMoving");
                rb.isKinematic = true;
                movableObj.IsMoving = false;
                hit.collider.transform.parent = _movableObjParent;
            }
            else if (hit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb2))
            {
                //used to set kinematic of the box because if kinematic is false the box won't be able to be moved by player
                rb2.isKinematic = true;
                _grabbedObj = hit.collider.gameObject;
                hit.collider.transform.parent = _movableObjParent;
            }
            else if(hit.collider.transform.parent) //use w/ bear trap or other objs that have parent
            {
                hit.collider.transform.parent.parent = _movableObjParent;
                print("grab parent");
            }
            else
            {
                print("grab obj");
                hit.collider.transform.parent = _movableObjParent;
            }

            _isGrabbing = true;
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;

        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
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
            else if (_grabbedObj != null && _grabbedObj.TryGetComponent<Rigidbody>(out Rigidbody rb3) && !_grabbedObj.TryGetComponent<RockController>(out RockController rockController))
            {
                rb3.isKinematic = false;
                _grabbedObj = null;
            }
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _movableObjParent.DetachChildren();     //Detach all children inside this parent
            _isGrabbing = false;
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
    }

    void RollRock()
    {
        RaycastHit hit;
        bool IsRockFound =
            Physics.Raycast(_movableObjDetectorCenter.position, Vector3.forward, out hit, _detectorRollRayDistance, _movableObjLayerMask) ||
            Physics.Raycast(_movableObjDetectorCenter.position, Vector3.back, out hit, _detectorRollRayDistance, _movableObjLayerMask);
        if (!_isGrabbing && IsRockFound && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.LeftControl))
        {
            if (hit.collider.gameObject.TryGetComponent<RockController>(out RockController rockController))
            {
                _grabbedObj = hit.collider.gameObject;
                rockController.IsRolled = true;
            }
            
            _isGrabbing = true;
            print(_grabbedObj);
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;

        }
        else if(Input.GetKeyUp(KeyCode.LeftControl) || !IsRockFound)
        {
            if (_grabbedObj && _grabbedObj.gameObject.TryGetComponent<RockController>(out RockController rockController))
            {
                rockController.IsRolled = false;
                _grabbedObj = null;
                _movableObjParent.DetachChildren();     //Detach all children inside this parent
            }
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _isGrabbing = false;
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

                //_rb.velocity = new Vector3(/*_rb.velocity.x + */0, /*_rb.velocity.y + */_slidingSpeedY, _slidingSpeedX);
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
            
        }*/
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

    private void LedgeClimb()
    {
        //use vertical input or jump to make _hanging false so that when player moves up/down or jump
        //they will get out off the ledge
        if(_rb.velocity.y < 0 && !_hanging)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up * _lineDownStartLength) + transform.forward * _lineDownSpace;
            Vector3 lineDownEnd = (transform.position + Vector3.up * _lineDownEndLength) + transform.forward * _lineDownSpace;
            Debug.DrawLine(lineDownStart, lineDownEnd);
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit);
            if (downHit.collider != null && downHit.collider.gameObject.CompareTag(TagManager.Ledge))
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - _lineFwdStartLength, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - _lineFwdEndLength, transform.position.z) +
                                     transform.forward;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit);
                Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if (fwdHit.collider != null && fwdHit.collider.gameObject.CompareTag(TagManager.Ledge))
                {
                    print("hang");
                    _rb.useGravity = false;
                    _rb.velocity = Vector3.zero;
                    // _rb.isKinematic = true;

                    _hanging = true;
                    print("in lEdge() : " + _rb.velocity);

                    //play animation

                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * _offsetNumMulFwd + transform.up * _offsetNumMulUp;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward =
                        -fwdHit.normal; //-fwdHit.normal make player face obj by checking the direction of the obj then make it negative

                }
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(_groundDetector.position, Vector3.down, _groundDetectorRayDistance, _groundLayerMask))
        {
            IsOnGround = true;
        }
        else
        {
            IsOnGround = false;
        }
    }

     
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.name == "Slider")
        {
            _isSlideOff = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject.name == "Slider")
        {
            _isSlideOff = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_movableObjDetectorCenter.position, new Vector3(_movableObjDetectorCenter.position.x, _movableObjDetectorCenter.position.y, _movableObjDetectorCenter.position.z  + _detectorRollRayDistance));
    }
}
