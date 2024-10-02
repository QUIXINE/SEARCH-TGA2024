using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class ReleaserDoor : MonoBehaviour
{
    public bool IsDoorOpening;
    public bool IsDoorOpened;
    public bool IsDoorClosing;
    [SerializeField] private Vector3 _downPosition;
    [SerializeField] private Vector3 _upPosition;
    [SerializeField] private float _zAxisUpPos;
    [SerializeField] private float _openCloseSpeed;

    private void Start()
    {
        _downPosition = transform.localPosition;
        _upPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _zAxisUpPos);
    }

    public IEnumerator OpenDoor()
    {
        while (IsDoorOpening)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                _upPosition, _openCloseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _upPosition) <= 0.0001f)
            {
                IsDoorOpened = true;
                IsDoorOpening = false;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    
    public IEnumerator CloseDoor()
    {
        while (IsDoorClosing)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                _downPosition, _openCloseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _downPosition) <= 0.0001f)
            {
                IsDoorClosing = false;
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }
}
