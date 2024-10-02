using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WindZone : MonoBehaviour
{
    [SerializeField] private Animator _windMillAnimator;
    [SerializeField] private List<GameObject> _logs;
    [SerializeField] private List<Transform> _logsPos;
    [SerializeField] private float _logAngle;
    [SerializeField] private float _windForce;
    [SerializeField] private float _nextTimeToStorm;
    [SerializeField] private float _nextTimeForLogFloat;
    private float _nextTimeToStormDecreased;
    private float _nextTimeForLogFloatDecreased;
    private bool _isLogComedOut;

    private void Start()
    {
        _nextTimeToStormDecreased = _nextTimeToStorm;
    }

    private void Update()
    {
        // if enable from trigger can't work as I want then use bool to check instead
        if (_nextTimeToStormDecreased > 0 && _nextTimeToStormDecreased > 2)
        {
            if (_nextTimeToStormDecreased > 2 && _nextTimeToStormDecreased < 3)
            {
                // Play animtion
                _windMillAnimator.enabled = true;
            }
            _nextTimeToStormDecreased -= Time.deltaTime;
        }
        // 2 is the time to call the wind 7 - 5 = 2, default time now is 7, wind comes out every 5 sec. 
        else if (_nextTimeToStormDecreased <= 2 && _nextTimeToStormDecreased > 0)
        {
            // ChangeEveryLogPos();
            
            if (!_isLogComedOut)
            {
                print("wind comes out");
                _isLogComedOut = true;
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
            _nextTimeToStormDecreased -= Time.deltaTime;

            //_nextTimeToStormDecreased = _nextTimeToStorm;
        }
        // log comes out when time reaches 2
        else if (_nextTimeToStormDecreased <= 0)
        {
            ChangeEveryLogPos();
            _nextTimeToStormDecreased = _nextTimeToStorm;
            
            _windMillAnimator.enabled = true;
            // _nextTimeForLogFloatDecreased = _nextTimeForLogFloat;
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
