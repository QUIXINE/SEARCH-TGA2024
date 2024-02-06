using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CabinRotation : MonoBehaviour 
{
    //Try use delegate to store function of cabin and wheel rotation, and put these logic in another class to use it to stop rotation at the same time
    //and to (clean code) single responsibility the code
    [SerializeField] private float _stopRotationTimeDefault = 0;
    [SerializeField] private float _stopRotationTime = 0;
    [SerializeField] private float _nextStopRotationTimeDefault;
    [SerializeField] private float _nextStopRotationTime;
    [SerializeField] private float _getDownRotation;
    [SerializeField] private float _getUpRotation;
    [SerializeField] private Transform _raycastTransform;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _wheel;
    [SerializeField] private WheelRotation _wheelRotation;

    //Rotate Floor Mechanic
    [FormerlySerializedAs("_FerrisStopCollider")]
    [Tooltip("collider of ferris stop collider in Floor Mechanic")]
    [SerializeField] private Collider _ferrisStopCollider;
    [SerializeField] private Collider _deliverLiftStopCollider;
    [SerializeField] private float _floorRotateSpeed;
    [SerializeField] private float _dividingTime;
    private bool _isFloorUp;
    private bool _isFloorDown;
    private GameObject _floorMechanic;
    
    
    private void Start() 
    {
        _wheelRotation = _wheel.GetComponent<WheelRotation>();
    }
    
    private void Update()
    {
        RotateFloorMechanic();
    }

    private void RotateFloorMechanic()
    {
        //Get down
        if(_stopRotationTime <= 0)
        {
            
            RotateCabin();
            if(!_wheelRotation.enabled)
                _wheelRotation.enabled = true;
            if(_nextStopRotationTime >= 0)
                _nextStopRotationTime -= Time.deltaTime;
        }
        
        //Get up
        else if(_stopRotationTime >= 0)
        {
            _wheelRotation.enabled = false;
            _stopRotationTime -= Time.deltaTime;
        }

        //Get Down
        if(_stopRotationTime <= _stopRotationTimeDefault/_dividingTime && _isFloorDown && _floorMechanic != null)
        {
            Vector3 direction = new Vector3(_floorMechanic.transform.rotation.eulerAngles.x, _floorMechanic.transform.rotation.eulerAngles.y, _getDownRotation);
            Quaternion targetRotation = Quaternion.Euler(direction);
            _floorMechanic.transform.rotation = Quaternion.Slerp(_floorMechanic.transform.rotation, targetRotation,
                Time.deltaTime * _floorRotateSpeed);
            
            //Get Down Finish
            if (Quaternion.Angle(_floorMechanic.transform.rotation, targetRotation) <= 2f)
            {
                // _floorMechanic.transform.eulerAngles = Vector3.Lerp(_floorMechanic.transform.eulerAngles, to, _floorRotateSpeed * Time.deltaTime);
                /*_floorMechanic.transform.Rotate(_floorMechanic.transform.rotation.x,
                    _floorMechanic.transform.rotation.y, _floorRotateSpeed * Time.deltaTime);*/
                _floorMechanic.transform.rotation = targetRotation;
                _isFloorDown = false;
                _floorMechanic = null;
            }
            
        }
        //Get Up
        else if(_isFloorUp &&_floorMechanic != null)
        {
            Vector3 direction = new Vector3(_floorMechanic.transform.rotation.x, _floorMechanic.transform.rotation.y, _getUpRotation);
            Quaternion targetRotation = Quaternion.Euler(direction);
            _floorMechanic.transform.rotation = Quaternion.Slerp(_floorMechanic.transform.rotation, targetRotation,
                Time.deltaTime * _floorRotateSpeed);
            
            //Get Up Finish
            if (Quaternion.Angle(_floorMechanic.transform.rotation, targetRotation) <= 2f)
            {
                _floorMechanic.transform.rotation = targetRotation;
                _isFloorUp = false;
                _isFloorDown = true;
            }
        }
    }

    private void RotateCabin()
    {
        transform.localRotation = Quaternion.FromToRotation(-transform.parent.up, Vector3.down);  
        //wheel_Rotation.enabled = true;
        //stop rotate by checking if raycast detects the ground, and setting stopRotationTime to desired time
        if(IsGroundFounded() && _nextStopRotationTime <= 0)
        {
            _isFloorUp = true;
            _stopRotationTime = _stopRotationTimeDefault;
            _nextStopRotationTime = _nextStopRotationTimeDefault;   //used this so that it will not stop while the wheel is rotating and already stop but raycast still detects the ground
        }
    }

    //Check if raycast found ground layer
    //Should check trigger collider at to surface of ground instead because the cabin has to stop at the surface and it's easier (if don't need nextStopRotationTime)
    private bool IsGroundFounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(_raycastTransform.position, Vector3.left, out hit, _raycastDistance, _groundLayer) 
           || Physics.Raycast(_raycastTransform.position, Vector3.right, out hit, _raycastDistance, _groundLayer))
        {
            if(hit.collider.CompareTag(TagManager.FerrisStopDestination) && hit.collider == _ferrisStopCollider)
            {
                _floorMechanic = hit.collider.gameObject.transform.parent.gameObject;
                return true;
            }
            else if (hit.collider.CompareTag(TagManager.FerrisStopDestination) && hit.collider == _deliverLiftStopCollider)
            {
                return true;
            }
        }
        else
        {
            _floorMechanic = null;
            _isFloorUp = false;
        }
        return false;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_raycastTransform.position, new Vector3(_raycastTransform.position.x + _raycastDistance, _raycastTransform.position.y, _raycastTransform.position.z));
        Gizmos.DrawLine(_raycastTransform.position, new Vector3(_raycastTransform.position.x - _raycastDistance, _raycastTransform.position.y, _raycastTransform.position.z));
    }

    /* private void LateUpdate() 
    {
        transform.localRotation = Quaternion.FromToRotation(-transform.parent.up, Vector3.down);  
    } */
    /*Set
            /*Quaternion quaternion = new Quaternion();
            quaternion.SetFromToRotation(new Vector3(_floorMechanic.transform.rotation.x,
                    _floorMechanic.transform.rotation.y,
                    _floorMechanic.transform.rotation.z)
                , new Vector3(_floorMechanic.transform.rotation.x, _floorMechanic.transform.rotation.y, 0f));
            transform.rotation = quaternion * transform.rotation;#1#


            /*_floorMechanic.transform.eulerAngles = new Vector3(_floorMechanic.transform.rotation.x,
                                                _floorMechanic.transform.rotation.x, _getDownRotation);#1#
            // _floorMechanic.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.up);
            //From child
            /*_floorMechanic.transform.rotation = Quaternion.FromToRotation(
                new Vector3(_floorMechanic.transform.rotation.x, _floorMechanic.transform.rotation.y,
                    _floorMechanic.transform.rotation.z)
                , new Vector3(_floorMechanic.transform.rotation.x, _floorMechanic.transform.rotation.y, 0));#1#
            //From parent
            /* _floorMechanic.transform.parent.rotation = Quaternion.FromToRotation(
                new Vector3(_floorMechanic.transform.parent.rotation.x, _floorMechanic.transform.parent.rotation.y,
                    _floorMechanic.transform.parent.rotation.z)
                , new Vector3(_floorMechanic.transform.parent.rotation.x, _floorMechanic.transform.parent.rotation.y, 0));#1#*/
}