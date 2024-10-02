using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WindZoneTest : MonoBehaviour
{
    [SerializeField] private Animator[] _windMillAnimators;
    [SerializeField] private Collider[] _WindZonesForPlayer;
    [SerializeField] private List<GameObject> _logs;
    [SerializeField] private List<GameObject> _winds;
    [SerializeField] private List<Transform> _logsPos;
    [SerializeField] private float _logAngle;
    [SerializeField] private float _windForce;
    [FormerlySerializedAs("_Timer01")] [SerializeField] private float _timer01;
    [FormerlySerializedAs("_Timer02")] [SerializeField] private float _timer02;
    private float _timer01Decreased;
    private float _timer02Decreased;
    private bool _isWindBlowing;
    private bool _isLogComedOut;

    private void Start()
    {
        _timer01Decreased = _timer01;
        _timer02Decreased = _timer02;
    }

    private void Update()
    {
        // if enable from trigger can't work as I want then use bool to check instead
        if (!_isWindBlowing && _timer01Decreased > 0 && _timer01Decreased > 2)
        {
            /*if (_timer01Decreased > 2 && _timer01Decreased < 3)
            {
                // Play animtion
                _windMillAnimator.enabled = true;
            }*/
            _timer01Decreased -= Time.deltaTime;
        }
        // 2 is the time to call the wind 7 - 5 = 2, default time now is 7, wind comes out every 5 sec. 
        else if (!_isWindBlowing && _timer01Decreased <= 2 && _timer01Decreased > 0)
        {
            // ChangeEveryLogPos();
            
            if (!_isLogComedOut)
            {
                print("wind comes out");
                _isLogComedOut = true;
            }

            foreach (var animator in _windMillAnimators)
            {
                if (!animator.enabled)
                {
                    // Play animation
                    print("play fan animation");
                    animator.enabled = true;
                }
            }
            
            
            /*if (_nextTimeToStormDecreased > 0 && _nextTimeToStormDecreased < 1.5F)
                {
                    // Play animtion
                    _windMillAnimator.enabled = true;
                }
                else
                {
                    _windMillAnimator.enabled = false;
                }*/
            _timer01Decreased -= Time.deltaTime;

            //_nextTimeToStormDecreased = _nextTimeToStorm;
        }
        // log comes out when time reaches 2
        else if (!_isWindBlowing && _timer01Decreased <= 0)
        {
            ChangeEveryLogPos();
            print("Wind is blowing.");

            // Play wind particle
            foreach (var wind in _winds)
            {
                wind.gameObject.SetActive(true);
            }
            // Enable wind zones for player to make player takes damage
            foreach (var col in _WindZonesForPlayer)
            {
                print("Enable wind zones for player");
                col.enabled = true;
            }
            
            _timer02Decreased = _timer02;
            _isWindBlowing = true;

            // _windMillAnimator.enabled = true;
            // _nextTimeForLogFloatDecreased = _nextTimeForLogFloat;
        }


        if (_isWindBlowing && _timer02Decreased > 0)
        {
            _timer02Decreased -= Time.deltaTime;
        }
        else if (_isWindBlowing && _timer02Decreased <= 0)
        {
            print("stop blowing");
            _timer01Decreased = _timer01;
            foreach (var wind in _winds)
            {
                wind.gameObject.SetActive(false);
            }
            foreach (var animator in _windMillAnimators)
            {
                // Stop animation
                print("fan animation");
                animator.enabled = false;
            }
            
            // Disable wind zones for player
            foreach (var col in _WindZonesForPlayer)
            {
                print("Disable wind zones for player");
                col.enabled = false;
            }
            _isWindBlowing = false;
        }
        /*if (_nextTimeForLogFloatDecreased > 0)
        {
            _nextTimeForLogFloatDecreased -= Time.deltaTime;
        }
        else if (_nextTimeForLogFloatDecreased <= 0)
        {
            ChangeEveryLogPos();
            _nextTimeForLogFloatDecreased = _nextTimeForLogFloat;
        }*/
    }
    
    

    private void ChangeEveryLogPos()
    {
        // 
        // random pos
        // int i;
        // int a = Random (0,5)
        // i = a
        // pos = pos[a]
        //
        List<int> posNum = new List<int>();
        for (int i = 0; i < _logs.Count; i++)
        {
            int a = Random.Range(0, _logsPos.Count);
            while (posNum.Any(x => x == a))
            {
                a = Random.Range(0, _logsPos.Count);
            }
            posNum.Add(a);
            _logs[i].GetComponent<Rigidbody>().useGravity = false;
            _logs[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            _logs[i].GetComponent<Rigidbody>().isKinematic = true;
            _logs[i].GetComponent<Rigidbody>().isKinematic = false;
            _logs[i].transform.position = _logsPos[a].position;
            _logs[i].transform.rotation = _logsPos[a].rotation;
        }
    }
    

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.Woods))
        {
            var hitObj = col.gameObject;
            var rb = hitObj.GetComponent<Rigidbody>();
            var dir = -hitObj.transform.forward;
            Vector3 rotated = Quaternion.Euler(_logAngle, 0, 0) * Vector3.one;
            float angleOfMovement = Mathf.Atan2(rotated.x, rotated.z) * Mathf.Rad2Deg;
            Quaternion currentRotation = hitObj.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(angleOfMovement, 0, 0);
            rb.AddForce(dir * _windForce);
            hitObj.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, 1 * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        var rb = col.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        
    }
}
