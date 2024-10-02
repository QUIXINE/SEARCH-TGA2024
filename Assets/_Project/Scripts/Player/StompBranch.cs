using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StompBranch : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isBranchFound;
    [SerializeField] private Transform _branchDetectorTransform;
    [SerializeField] private float _detectorRayDistance;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Stomp();
    }

    private void Stomp()
    {
        _isBranchFound = Physics.Raycast(_branchDetectorTransform.position, Vector3.down, out RaycastHit hit,_detectorRayDistance);
        if (_isBranchFound)
        {
            if (hit.collider.CompareTag(TagManager.Branch) && hit.rigidbody.isKinematic && _rb.velocity.y < -0.5f)
            {
                hit.rigidbody.isKinematic = false;
                hit.collider.transform.rotation = Quaternion.Euler(0,0,0);
            }
        }
    }
}
