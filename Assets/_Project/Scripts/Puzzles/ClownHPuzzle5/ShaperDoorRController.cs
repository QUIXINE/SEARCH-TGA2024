using System.Collections;
using UnityEngine;

public class ShaperDoorRController : MonoBehaviour
{
    public bool IsCollidedChangerToOpen;
    public bool IsCollidedChangerToClose;
    [SerializeField] private float _doorMoveSpeed;
    [SerializeField] private Vector3 _upPos;
    private Vector3 _downPos;

    private void Start()
    {
        _downPos = transform.localPosition;
    }

    public IEnumerator Open()
    {
        while (IsCollidedChangerToOpen)
        {
            print("Open");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _upPos, _doorMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _upPos) <= 0.1f)
            {
                IsCollidedChangerToOpen = false;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    
    public IEnumerator Close()
    {
        while (IsCollidedChangerToClose)
        {
            print("Close");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _downPos, _doorMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _downPos) <= 0.1f)
            {
                IsCollidedChangerToClose = false;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
