using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private float _horizontalInput;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    
    [Header("Move Object Check")]
    [SerializeField] private Transform _movableObjDetectorLeft, _movableObjDetectorRight;
    [SerializeField] private Transform _movableObjParent;
    [SerializeField] private float _detectorRayDistance;
    private bool _isNearMovableObject;
    private bool _isGrabbing;
    private Vector3 _playerYPos;

    //Slide Check
    [Header("Slide Check")]
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private float _groundDetectorRayDistance;
    [SerializeField] private float _slidingSpeedX;
    [SerializeField] private float _slidingSpeedY;
    [SerializeField] private float _slopeLimit;
    [SerializeField] private float _ySpeed;
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
    

    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerYPos = new Vector3(0, transform.position.y, 0);
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        Move();
        Jump();
        PushAndPull();
        SlideDown();
        GroundCheck();
    }
    
    private void Move()
    {
        _rb.velocity = new Vector3((_horizontalInput * _speed) * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_hanging)
            {
                _rb.useGravity = true;
                // _rb.isKinematic = false;
                _hanging = false;
                _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
                print("in Jump() : " + _rb.velocity);

            }
            else
            {
                _rb.AddForce((Vector3.up * _jumpForce) * Time.deltaTime, ForceMode.Impulse);
                LedgeClimb();
            }
        }
    }

    private void PushAndPull()
    {
        RaycastHit hit;
        _isNearMovableObject = Physics.Raycast(_movableObjDetectorLeft.position, Vector3.left, out hit,_detectorRayDistance) || Physics.Raycast(_movableObjDetectorLeft.position, Vector3.right, out hit,_detectorRayDistance)
                               || Physics.Raycast(_movableObjDetectorRight.position, Vector3.right, out hit,_detectorRayDistance)|| Physics.Raycast(_movableObjDetectorRight.position, Vector3.left, out hit,_detectorRayDistance)? true : false;
       
        print(_isNearMovableObject);
        if (!_isGrabbing &&_isNearMovableObject && hit.collider.CompareTag(TagManager.MovableItem) && Input.GetKey(KeyCode.RightControl))
        {
            _rb.constraints |= RigidbodyConstraints.FreezePositionY;
            //hit.collider.transform.parent.parent = _movableObjParent; --> original, has no if-else under
            if(hit.collider.transform.parent) //use w/ bear trap or other objs that have parent
            {
                hit.collider.transform.parent.parent = _movableObjParent;
                print("grab parent");
            }
            else
            {
                hit.collider.transform.parent = _movableObjParent;
                print("grab obj");
            }

            _isGrabbing = true;
            // Vector3 newPos = new Vector3(hit.collider.transform.parent.transform.position.x,0,0);
            // hit.collider.transform.parent.transform.position = newPos;

        }
        else if(Input.GetKeyUp(KeyCode.RightControl))
        {
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _movableObjParent.DetachChildren();     //Detach all children inside this parent
            _isGrabbing = false;
            print("release 1");
        }
        /*else
        {
            _movableObjParent.DetachChildren();     //Detach all children inside this parent
            _isGrabbing = false;
            print("release 2");
        }*/
    }

    private void SlideDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(_groundDetector.position, Vector3.down, out hit, _groundDetectorRayDistance))
        {
            if (hit.collider.gameObject.name == "Slider" && !_isSlideOff)
            {
                print("hit");
                if (_horizontalInput <= 0)
                {
                    _horizontalInput = 1;
                }
                _rb.velocity = new Vector3(/*_rb.velocity.x + */0, /*_rb.velocity.y + */_slidingSpeedY, _slidingSpeedX);
            }
            
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
        if(_rb.velocity.y < 0 && !_hanging)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up * _lineDownStartLength) + transform.forward * _lineDownSpace;
            Vector3 lineDownEnd = (transform.position + Vector3.up * _lineDownEndLength) + transform.forward * _lineDownSpace;
            Debug.DrawLine(lineDownStart, lineDownEnd);
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - _lineFwdStartLength, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - _lineFwdEndLength, transform.position.z) +
                                     transform.forward;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if (fwdHit.collider != null)
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
            print("col enter");
            _isSlideOff = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.gameObject.name == "Slider")
        {
            print("col exit");
            _isSlideOff = true;
        }
    }
    
}
