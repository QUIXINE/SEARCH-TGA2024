using UnityEngine;
using UnityEngine.Serialization;

public class WoodClamp : MonoBehaviour
{
    [Header("Circle Checker Setup")]
    [SerializeField] private float _outerCircleRadius, _innerCircleRadius;
    [SerializeField] private float _outerCircleDistance, _innerCircleDistance;
    [SerializeField] private FirePitOpenClose[] _firePitOpenClose;
    [Tooltip("close fire pit after Wait To Move Left Time reach to this")]
    [SerializeField] private float _timeToCloseFirePit;
    
    [FormerlySerializedAs("_target")]
    [Header("Animation Rigging Effector")]
    [SerializeField] private Transform _effector;
    
    [Header("Effector Position Setting")]
    [SerializeField] private Vector3 _targetStartPosition;
    [SerializeField] private Vector3 _targetStraightPosition;
    
    [Header("Clamp Position Setting")]
    private Vector3 _clampStartPosition;
    private Vector3 _clampGetUpPosition;
    [FormerlySerializedAs("_clampYPosToGetUp")]
    [Tooltip("plus this y pos to root's transform position to get up")]
    [SerializeField] private float _yPosToGetUp;
    private Vector3 _clampGetUpPositionForPlayer;   //Clamp Get Up Position For Player will move lower than Clamp Get Up Position
    [Tooltip("In case of player is grabbed, plus this y pos to root's transform position to get up")]
    [SerializeField] private float _yPosToGetUpIfGrabPlayer;
    private Vector3 _clampGetRightPosition;
    [Tooltip("In case of player is grabbed, plus this y pos to root's transform position to get to the right pos")]
    [SerializeField] private float _xPosToGetRight;
    [Tooltip("Clamp moving speed")]
    [SerializeField] private float _maxDistanceDelta;
    [Tooltip("Clamp grabbing speed when found player")]
    [SerializeField] private float _grabSpeed;
    [SerializeField] private float _waitToMoveDownTime;
    [SerializeField] private float _waitToMoveLeftTime;
    private float _waitToMoveDownTimeDecreased;
    private float _waitToMoveLeftTimeDecreased;
    private bool _isAbleToGrab = true;
    private bool _isPlayerGrabbed;
    private bool _isWoodGrabbed;
    private bool _isMove;
    private bool _isGoingUp;
    private bool _isGoingDown;
    private bool _isGoingRight;
    private bool _isGoingLeft;

    private GameObject _grabbedObj;
    // Start is called before the first frame update
    void Start()
    {
        _targetStartPosition = _effector.transform.localPosition;
        _targetStraightPosition = new Vector3(_effector.transform.localPosition.x - 0.2f,
            _effector.transform.localPosition.y + 0.005f, _effector.transform.localPosition.z);
        _clampStartPosition = transform.root.position;
        _clampGetUpPosition = new Vector3(transform.root.position.x, transform.root.position.y + _yPosToGetUp, transform.root.position.z);
        _clampGetUpPositionForPlayer = new Vector3(transform.root.position.x, transform.root.position.y + _yPosToGetUpIfGrabPlayer, transform.root.position.z);
        _clampGetRightPosition = new Vector3(transform.root.position.x + _xPosToGetRight, _clampGetUpPositionForPlayer.y, transform.root.position.z);
        _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
        _waitToMoveLeftTimeDecreased = _waitToMoveLeftTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Look at, after obj is child to this obj then stop look at and lerp this obj's parent position
        IsObjInOuterCircle();
    }

    private void IsObjInOuterCircle()
    {
        bool isHit = Physics.SphereCast(transform.position, _outerCircleRadius, Vector3.down, out RaycastHit hitInfo,
            _outerCircleDistance);
        if (isHit && hitInfo.collider.gameObject.CompareTag(TagManager.MovableItem))
        {
            // _target.LookAt(hitInfo.collider.transform);
            if (_isAbleToGrab)
            {
                _grabbedObj = hitInfo.collider.gameObject.transform.parent.gameObject;
                _isPlayerGrabbed = false;
                _isWoodGrabbed = true;
                _isAbleToGrab = false;
                if (!_isGoingDown)
                {
                    _isMove = true;
                }
            }
        }
        if (isHit && hitInfo.collider.gameObject.CompareTag(TagManager.Player))
        {
            // _target.LookAt(hitInfo.collider.transform);
            if (_isAbleToGrab)
            {
                _grabbedObj = hitInfo.collider.gameObject;
                _isWoodGrabbed = false;
                _grabbedObj.GetComponent<Animator>().SetBool("IsWalking", false);
                _grabbedObj.GetComponent<PlayerController>().HorizontalInput = 0;
                _grabbedObj.GetComponent<PlayerController>().enabled = false;
                _isPlayerGrabbed = true;
                _isAbleToGrab = false;
                if (!_isGoingLeft)
                {
                    _isMove = true;
                }
            }
                
        }
        /*else if (isHit)
        {
            print(hitInfo.collider.gameObject.name);
        }*/

        if (_isMove)
        {
            MoveEffector();
        }
        if (_isWoodGrabbed)
        {
            if (_isGoingUp)
            {
                MoveUp();
            }
            else if (_isGoingDown)
            {
                MoveDown();
            }
        }
        else if (_isPlayerGrabbed)
        {
            if (_isGoingUp)
            {
                transform.root.position = Vector3.MoveTowards(transform.root.position, _clampGetUpPositionForPlayer, _maxDistanceDelta * Time.deltaTime);
                _effector.localPosition = Vector3.MoveTowards(_effector.localPosition, _targetStraightPosition, _maxDistanceDelta * Time.deltaTime);
                if (Vector3.Distance(transform.root.position, _clampGetUpPositionForPlayer) <= 0.01f)
                {
                    _isGoingRight = true;
                    _isGoingUp = false;
                }
            }
            else if (_isGoingRight)
            {
                //Open the fire pit
                _firePitOpenClose[0].IsPlayerClamped = true;
                _firePitOpenClose[1].IsPlayerClamped = true;
                
                transform.root.position = Vector3.MoveTowards(transform.root.position, _clampGetRightPosition, _maxDistanceDelta * Time.deltaTime);
                if (Vector3.Distance(transform.root.position, _clampGetRightPosition) <= 0.01f)
                {
                    _grabbedObj.transform.parent = null;
                    _grabbedObj.GetComponent<Rigidbody>().useGravity  = true;
                    _grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
                    //wait to get back
                    _waitToMoveDownTimeDecreased -= Time.deltaTime;
                    if (_waitToMoveDownTime <= 0)
                    {
                        //is down = true, is up = false
                        _isGoingLeft = true;
                        _isGoingRight = false;
                        _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
                    }
                }
            }
            else if (_isGoingLeft)
            {
                _waitToMoveLeftTimeDecreased -= Time.deltaTime;
                if (_waitToMoveLeftTimeDecreased <= _timeToCloseFirePit && _waitToMoveLeftTimeDecreased > 0)
                {
                    _firePitOpenClose[0].IsPlayerClamped = false;
                    _firePitOpenClose[1].IsPlayerClamped = false;
                    print(_firePitOpenClose[0].IsPlayerClamped);
                }
                else if (_waitToMoveLeftTimeDecreased <= 0)
                {
                    //is down = true, is up = false
                    Vector3 moveDown = new Vector3(transform.root.position.x, _clampStartPosition.y,
                        transform.root.position.z);
                    transform.root.position = Vector3.MoveTowards(transform.root.position, moveDown, _maxDistanceDelta * Time.deltaTime);
                    if (Vector3.Distance(transform.root.position, moveDown) <= 0.01f)
                    {
                        transform.root.position = Vector3.MoveTowards(transform.root.position, _clampStartPosition, _maxDistanceDelta * Time.deltaTime);
                        if (Vector3.Distance(transform.root.position, _clampStartPosition) <= 0.01f)
                        {
                            _waitToMoveLeftTimeDecreased = _waitToMoveLeftTime;
                            _isAbleToGrab = true;
                            _isGoingLeft = false;
                            _isPlayerGrabbed = false;
                        }
                    }
                }
            }
        }
        
    }

    private void MoveDown()
    {
        transform.root.position = Vector3.MoveTowards(transform.root.position, _clampStartPosition, _maxDistanceDelta * Time.deltaTime);
        if (Vector3.Distance(transform.root.position, _clampStartPosition) <= 0.01f)
        {
            _effector.localPosition = Vector3.MoveTowards(_effector.localPosition, _targetStartPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(_effector.localPosition, _targetStartPosition) <= 0.01f)
            {
                _isWoodGrabbed = false;
                _isAbleToGrab = true;
                _isGoingDown = false;
            }
        }
    }

    private void MoveUp()
    {
        transform.root.position = Vector3.MoveTowards(transform.root.position, _clampGetUpPosition, _maxDistanceDelta * Time.deltaTime);
        _effector.localPosition = Vector3.MoveTowards(_effector.localPosition, _targetStraightPosition, _maxDistanceDelta * Time.deltaTime);
        if (Vector3.Distance(transform.root.position, _clampGetUpPosition) <= 0.01f)
        {
            _grabbedObj.GetComponent<WoodPooled>().IsWoodReadyToGetBack = true;
            //wait to get back
            _waitToMoveDownTimeDecreased -= Time.deltaTime;
            if (_waitToMoveDownTimeDecreased <= 0)
            {
                //is down = true, is up = false
                _isGoingDown = true;
                _isGoingUp = false;
                _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
            }
        }
    }

    private void MoveEffector()
    {
        _effector.position =
            Vector3.MoveTowards(_effector.position, _grabbedObj.transform.position, _grabSpeed * Time.deltaTime);
        //Check distance to go up, to get to straight pose 
        if (Vector3.Distance(_effector.position, _grabbedObj.transform.position) <= 0.1f)
        {
            //up, pose
            _grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
            _grabbedObj.GetComponent<Rigidbody>().useGravity = false;
            _grabbedObj.transform.parent = transform;
            _isGoingUp = true;
            _isMove = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _outerCircleRadius);
        Gizmos.DrawWireSphere(transform.position, _innerCircleRadius);
    }
}
