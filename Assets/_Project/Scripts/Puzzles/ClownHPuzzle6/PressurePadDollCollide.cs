using ClownHPuzzle6;
using UnityEngine;

public class PressurePadDollCollide : MonoBehaviour
{
    [SerializeField] private PressurePadDoor _pressurePadDoor;
    private void OnTriggerEnter(Collider col)
    {
        if (!_pressurePadDoor.IsDoorMoving)
        {
            if (col.gameObject.CompareTag(TagManager.MovableItem))
            {
                col.gameObject.transform.parent = null;
                col.enabled = false;
                _pressurePadDoor.ObjsCount++;
                _pressurePadDoor.PeopleAmountText.text = $"{_pressurePadDoor.ObjsCount}";
            }
        }
    }
    
    private void OnTriggerExit(Collider col)
        {
            if (!_pressurePadDoor.IsDoorMoving)
            {
                if (col.gameObject.CompareTag(TagManager.MovableItem))
                {
                    _pressurePadDoor.ObjsCount--;
                    _pressurePadDoor.PeopleAmountText.text = $"{_pressurePadDoor.ObjsCount}";
                }
            }
        }

        
}
