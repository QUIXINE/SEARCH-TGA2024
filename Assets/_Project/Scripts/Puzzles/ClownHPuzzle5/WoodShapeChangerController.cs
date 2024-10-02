using System;
using UnityEngine;

public class WoodShapeChangerController : MonoBehaviour
{
    [SerializeField] private ConveyorBelt _conveyorBelt;
    [Tooltip("Duration to stop conveyor belt from moving so that the wood/doll will be inside the shaper for the amount of time")]
    [SerializeField] private float _stopDuration;
    private float _stopDurationDecreased;
    private bool _isStopped;

    [Header("Open Shaper Door R")] 
    [SerializeField]  private ShaperDoorRController _shaperDoorRController;
    
    private void Update()
    {
        StopConveyorBelt();
    }

    private void StopConveyorBelt()
    {
        if (_isStopped)
        {
            if (_stopDurationDecreased > 0)
            {
                if (_conveyorBelt.IsMoving)
                {
                    _conveyorBelt.IsMoving = false;
                }
                _stopDurationDecreased -= Time.deltaTime;
            }
            else if (_stopDurationDecreased <= 0)
            {
                _conveyorBelt.IsMoving = true;
                _isStopped = false;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem))
        {
            Transform parent = col.gameObject.transform.parent;
            
            // get the 2nd child which is Doll Body
            Transform dollBody = parent.GetChild(1);
            
            if (dollBody.gameObject.name == "Doll Body" && col.gameObject.name == "Box")
            {
                print("WoodShapeChangerController Collides");
                dollBody.gameObject.SetActive(true);
                col.gameObject.SetActive(false);
                _shaperDoorRController.IsCollidedChangerToOpen = true;
                StartCoroutine(_shaperDoorRController.Open());
                _stopDurationDecreased = _stopDuration;
                _isStopped = true;
            }
        }
        else if (col.gameObject.CompareTag(TagManager.Player)) //if player get inside shaper, player will get kill
        {
            PlayerTakeDamage playerTakeDamage = col.gameObject.GetComponent<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage(gameObject.scene);
        }
    }
}
