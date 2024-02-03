using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Lever : MonoBehaviour, IButton
{
    public bool AreCagesReachTargetPos
    {
        get;
        private set;
    }
    [Header("Move Cages Down")]
    [SerializeField] private List<GameObject> _cages;
    [SerializeField] private List<Vector3> _cagesTargetPosition;
    [Tooltip("speed o movement")]
    [SerializeField] private float _maxDistanceDelta;
    private bool _isPulled;
    //use dto check how many cages reach target pos already
    private int _cageReachTargetAmount = 0;
    private int _cagesAmount;
    
    
    private void Start()
    {
        _cagesAmount = _cages.Count;
        for (int i = 0; i < _cages.Count; i++)
        {
            _cagesTargetPosition[i] = new Vector3(_cages[i].transform.position.x, _cagesTargetPosition[i].y, _cages[i].transform.position.z);
        }
    }

    private void Update()
    {
        ExecuteResult();
    }

    private void ExecuteResult()
    {
        if(_isPulled)
        {
            print("Pulled");
            for (int i = 0; i < _cages.Count; i++)
            {
                _cages[i].transform.position = Vector3.MoveTowards(_cages[i].transform.position, _cagesTargetPosition[i], _maxDistanceDelta * Time.deltaTime);
                if (_cages[i].transform.position == _cagesTargetPosition[i])
                {
                    _cageReachTargetAmount++;
                    _cages.RemoveAt(i);
                    _cagesTargetPosition.RemoveAt(i);
                }
            }
        }

        if (_cageReachTargetAmount == _cagesAmount)
        {
            _isPulled = false;
            AreCagesReachTargetPos = true;
            _cageReachTargetAmount = 0;
        }
        
        //Try to stop checking the if() and send signal to CageButtaon that it reaches target postion by setting property _isReachedTargetPosition
        /*if(cageReachTargetAmount < _cages.Length)
        {
            for (int i = 0; i < _cages.Length; i++)
            {
                if (_cages[i].transform.position == _cagesTargetPosition[i])
                {
                    cageReachTargetAmount++;
                }
            }
        }
            
        
        if (cageReachTargetAmount == _cages.Length)
        {
            _isPulled = false;
        }*/
    }

    public void Execute()
    {
        _isPulled = true;
    }
}
