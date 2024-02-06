using UnityEngine;
using UnityEngine.Serialization;

public class WormController : MonoBehaviour
{


    [FormerlySerializedAs("_target")] [Header("Animation Rigging Effector")] [SerializeField]
    private Transform _effector;

    [Header("Box Checker Setup")]
    [SerializeField] private Transform _boxCenter;
    [SerializeField] private Vector3 _halfExtents;
    [SerializeField] private float _maxDistance;

    [Header("Worm Position Setting")] 
    private Vector3 _startPosition;
    private Vector3 _moveUpPosition;
    [SerializeField] private float _moveUpDistanceY;

    [Tooltip("Worm moving speed")] [SerializeField]
    private float _maxDistanceDelta;

    [FormerlySerializedAs("_grabSpeed")] [Tooltip("Clamp grabbing speed when found player")] [SerializeField]
    private float _biteSpeed;

    [SerializeField] private float _waitToMoveDownTime;
    [SerializeField] private float _waitToMoveUpTime;
    private float _waitToMoveDownTimeDecreased;
    private float _waitToMoveUpTimeDecreased;
    private bool _isUp;
    private bool _isDown;

    [FormerlySerializedAs("_player")]
    [Header("Player/Food")] 
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _playerYPos;
    [SerializeField] private GameObject _player;
    
    void Start()
    {
        _startPosition = transform.root.position;
        _moveUpPosition = new Vector3(transform.root.position.x, transform.root.position.y + _moveUpDistanceY,
            transform.root.position.z);
        _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
        _waitToMoveUpTimeDecreased = _waitToMoveUpTime;
        _isUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Look at, after obj is child to this obj then stop look at and lerp this obj's parent position
        Movement();
    }

    private void Movement()
    {
        bool isHit = Physics.BoxCast(_boxCenter.position, _halfExtents, Vector3.up, out RaycastHit hitInfo,
            Quaternion.identity, _maxDistance);

        if (isHit)
        {
            _player = hitInfo.collider.gameObject;
        }
        else if (_player != null)
        {
            print($"isHit : {isHit}");
            _player.GetComponent<PlayerController>().enabled = false;
            _player.GetComponent<Rigidbody>().isKinematic = true;
            _player.GetComponent<Rigidbody>().useGravity = false;
            if (_isUp)
            {
                transform.root.position = Vector3.MoveTowards(transform.root.position, 
                    new Vector3(_playerTransform.position.x, _playerTransform.position.y + _playerYPos, _playerTransform.position.z), _biteSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.root.position, _playerTransform.position) <= 1f)
                {
                    MoveEffector();
                }
            }
            else if (_isDown)
            {
                transform.root.position = Vector3.MoveTowards(transform.root.position, 
                    new Vector3(_playerTransform.position.x, _playerTransform.position.y + _playerYPos, _playerTransform.position.z), _biteSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.root.position, _playerTransform.position) <= 1f)
                {
                    MoveEffector();
                }
            }
        }
        else if (!isHit && _player == null)
        {
            print($"isHit : {isHit}");
            if (_isUp)
            {
                MoveUp(isHit, hitInfo);
            }
            else if (_isDown)
            {
                MoveDown(isHit, hitInfo);
            }
        }
    }

    
    private void MoveUp(bool isHit, RaycastHit hitInfo)
    {
        //_target.localPosition = Vector3.MoveTowards(_target.localPosition, _targetStraightPosition, _maxDistanceDelta * Time.deltaTime);
        if (!isHit)
        {
            transform.root.position = Vector3.MoveTowards(transform.root.position, _moveUpPosition, _maxDistanceDelta * Time.deltaTime);
            //wait to get back
            if (Vector3.Distance(transform.root.position, _moveUpPosition) <= 0.01f)
            {
                _waitToMoveDownTimeDecreased -= Time.deltaTime;
                if (_waitToMoveDownTimeDecreased <= 0)
                {
                    //is down = true, is up = false
                    _isDown = true;
                    _isUp = false;
                    _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
                }
            }
        }
        /*else if (isHit)
        {
            /*transform.root.position = Vector3.MoveTowards(transform.root.position, _player.position, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(transform.root.position, _moveUpPosition) <= 1f)
            {
                MoveEffector();
            }#1#
        }*/
        //if player in range (detected by boxcast), then move worm rapidly towards player.
        //if worm is closer to player (if detected by boxcast and distance between player and head is close), moves effector to player

    }
    
    private void MoveDown(bool isHit, RaycastHit hitInfo)
    {
        if (!isHit)
        {
            transform.root.position = Vector3.MoveTowards(transform.root.position, _startPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(transform.root.position, _startPosition) <= 0.01f)
            {
                _waitToMoveDownTimeDecreased -= Time.deltaTime;
                if (_waitToMoveDownTimeDecreased <= 0)
                {
                    //is down = true, is up = false
                    _isDown = false;
                    _isUp = true;
                    _waitToMoveUpTimeDecreased = _waitToMoveUpTime;
                }
            }
        }
        /*else if (isHit)
        {
            transform.root.position = Vector3.MoveTowards(transform.root.position, _player.position, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(transform.root.position, _player.position) <= 1f)
            {
                MoveEffector();
            }
        }*/
        //if player in range (detected by boxcast), then move worm rapidly towards player.
        //if worm is closer to player (if detected by boxcast and distance between player and head is close), moves effector to player
    }

   

    private void MoveEffector()
    {
        _effector.gameObject.transform.position =
            Vector3.MoveTowards(_effector.position, _playerTransform.position, _biteSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCenter.position, _halfExtents/0.5f);
    }
}
