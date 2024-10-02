using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WormController : MonoBehaviour
{


    [Header("Animation Rigging Effector")]
    [SerializeField] private Transform _effector;

    [Header("Animation")] 
    [SerializeField] private Animator _animator;
    private int _attackAnimId = Animator.StringToHash("Attack");
    
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

    [Header("Player/Food")] 
    //Use FindGameObj instead of dragging in
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _playerYPos;
    [SerializeField] private GameObject _player;
    private bool _isHit;
    // private RaycastHit hitInfo;
    void Start()
    {
        _startPosition = transform.position;
        _moveUpPosition = new Vector3(transform.position.x, transform.position.y + _moveUpDistanceY,
            transform.position.z);
        _waitToMoveDownTimeDecreased = _waitToMoveDownTime;
        _waitToMoveUpTimeDecreased = _waitToMoveUpTime;
        _isUp = true;
        var player = FindObjectOfType<PlayerController>();
        _playerTransform = player.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Look at, after obj is child to this obj then stop look at and lerp this obj's parent position
        Movement();
    }

    private void FixedUpdate()
    {
        // _isHit = Physics.BoxCast(_boxCenter.position, _halfExtents, Vector3.up, out hitInfo,
        //     Quaternion.identity, _maxDistance);
    }

    private void Movement()
    {
        //if player in range (detected by boxcast), then move worm rapidly towards player.
        //if worm is closer to player (if detected by boxcast and distance between player and head is close), moves effector to player
        // bool isHit = Physics.BoxCast(_boxCenter.position, _halfExtents, Vector3.up, out RaycastHit hitInfo,
        //     Quaternion.identity, _maxDistance);

        if (!_isHit && _player == null)
        {
            if (_isUp)
            {
                MoveUp(_isHit);
            }
            else if (_isDown)
            {
                MoveDown(_isHit);
            }
        }
        else if (_player == _playerTransform.gameObject)
        {
            _player.GetComponent<PlayerController>().HorizontalInput = 0;
            _player.GetComponent<PlayerController>().enabled = false;
            _player.GetComponent<Rigidbody>().isKinematic = true;
            _player.GetComponent<Rigidbody>().useGravity = false;
            if (_isUp)
            {
                var pos = new Vector3(transform.position.x, _playerTransform.position.y + _playerYPos,
                    transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, 
                    pos, _biteSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pos) <= 0.01f)
                {
                    MoveEffector();
                }
            }
            else if (_isDown)
            {
                var pos = new Vector3(transform.position.x, _playerTransform.position.y + _playerYPos,
                    transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, 
                    pos, _biteSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pos) <= 0.01f)
                {
                    MoveEffector();
                }
            }
        }
        
    }

    
    private void MoveUp(bool isHit)
    {
        //_target.localPosition = Vector3.MoveTowards(_target.localPosition, _targetStraightPosition, _maxDistanceDelta * Time.deltaTime);
        if (!isHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveUpPosition, _maxDistanceDelta * Time.deltaTime);
            //wait to get back
            if (Vector3.Distance(transform.position, _moveUpPosition) <= 0.01f)
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

    }
    
    private void MoveDown(bool isHit)
    {
        if (!isHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, _maxDistanceDelta * Time.deltaTime);
            if (Vector3.Distance(transform.position, _startPosition) <= 0.01f)
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
    }

   

    private void MoveEffector()
    {
        _effector.gameObject.transform.position =
            Vector3.MoveTowards(_effector.position, _playerTransform.position, _biteSpeed * Time.deltaTime);
        if (Vector3.Distance(_effector.position, _playerTransform.position) <= 0.01f && !_animator.GetBool(_attackAnimId))
        {
            var takeDamage = _player.GetComponent<PlayerTakeDamage>();
            takeDamage.TakeDamage(gameObject.scene);
            _animator.SetBool(_attackAnimId, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _isHit = true;
            if (_player == null)
            {
                _player = other.gameObject;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCenter.position, _halfExtents/0.5f);
    }
}
