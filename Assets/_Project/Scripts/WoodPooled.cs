using UnityEngine;

public class WoodPooled : MonoBehaviour
{
    public bool IsWoodReadyToGetBack;
    public bool IsWoodInPool { get; private set;}
    [Tooltip("define y-axis position and use it to check if the Wood is out of boundary " +
             "while being clamped," + " then take it back to first position")]
    [SerializeField] private float _yPos;
    [SerializeField] private GameObject _box;
    [SerializeField] private GameObject _dollBody;
    [SerializeField] private Renderer _releaseBoxBtnRenderer;
    private Vector3 _startPosition;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWoodReadyToGetBack) //can't use because after be a child the position will lock unless the obj moves, Why? ask why because right now it works
        {
            print("moved");
            _dollBody.SetActive(false);
            _box.SetActive(true);
            _rb.isKinematic = true;
            _rb.useGravity = true;
            transform.position = _startPosition;
            transform.parent = null;
            _releaseBoxBtnRenderer.material.EnableKeyword("_EMISSION");
            IsWoodReadyToGetBack = false;
        }
        else if (transform.position == _startPosition && !IsWoodInPool)
        {
            IsWoodInPool = true;
        }
    }
}
