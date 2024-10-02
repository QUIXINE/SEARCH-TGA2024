using UnityEngine;

public class PlayerSlidesDown : MonoBehaviour
{
    [SerializeField] private LayerMask _slideLayer;
    [SerializeField] private float _rayDis = 3;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _slideLayer = LayerMask.GetMask("Obstacle");
    }

    private void FixedUpdate()
    {
        SlideCheck();
    }

    private void SlideCheck()
    {
        bool isSlideFound = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, _rayDis, _slideLayer);
        if (isSlideFound)
        {
            if (hitInfo.collider.gameObject.name == "Slider")
            {
                _playerController.IsSlideFound = true;
            }
            else
            {
                _playerController.IsSlideFound = false;
            }
        }
    }
}
