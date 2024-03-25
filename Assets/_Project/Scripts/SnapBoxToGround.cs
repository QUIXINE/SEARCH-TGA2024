 using UnityEngine;
 using UnityEngine.Serialization;

 public class SnapBoxToGround : MonoBehaviour
{
    [SerializeField] private ClownStateManager _clownStateManager;
    [SerializeField] private Animator _clownAnimator;
    [SerializeField] private Transform _rayPoint;
    [SerializeField] private Collider _colliderOfBox;
    [SerializeField] private float _rayDis;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isBoxOnGround;
    
  private void Update()
  {
      //Trigger animation
      /*for testing
           if (Input.GetKeyDown(KeyCode.E))
          {
              _animator.SetBool(_isPutDownAnimId, true);
          }
      */
      if (Physics.Raycast(_rayPoint.position, Vector3.down, out RaycastHit hitInfo, _rayDis, _groundLayer)
          && !_isBoxOnGround && _clownAnimator.GetBool(_clownStateManager.IsPuttingDownAnimId) && _clownAnimator.GetCurrentAnimatorStateInfo(0).IsName("PutDown") 
          && _clownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_clownAnimator.IsInTransition(0))
      {
          transform.parent = null;
          transform.position = hitInfo.point;
          transform.rotation = Quaternion.Euler(0,0,0);
          _clownStateManager.IsBoxPutDown = true;
          _colliderOfBox.isTrigger = false;
          // var rb = GetComponent<Rigidbody>().isKinematic = false;
          _isBoxOnGround = true;
      }
      
      //check animation's status
      /*for testing
           if (_animator.GetBool(_isPutDownAnimId) && _isBoxOnGround && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0))
          {
              print("Animation is finished");
              _animator.SetBool(_isPutDownAnimId, false);
          }
      */
  }
  
  private void OnDrawGizmos()
  {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(_rayPoint.position, new Vector3(_rayPoint.position.x, _rayPoint.position.y + Mathf.Abs(_rayDis) * -1, _rayPoint.position.z));
  }
}
