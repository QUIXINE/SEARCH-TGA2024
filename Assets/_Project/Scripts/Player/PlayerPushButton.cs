using UnityEngine;

public class PlayerPushButton : MonoBehaviour 
{
    private bool _isReadyToPush;
    private IButton _button;

    private void Update() 
    {
        if(_isReadyToPush && Input.GetKeyDown(KeyCode.RightControl))
        {
            _button.Execute();
        }    
    }
    private void OnTriggerEnter(Collider col) 
    {
        if(col.gameObject.CompareTag(TagManager.Button))
        {
            _button = col.gameObject.GetComponent<IButton>();
            _isReadyToPush = true;
        }
    }    

    private void OnTriggerExit(Collider col) 
    {
        if(col.gameObject.CompareTag(TagManager.Button))
        {
            _button = null;
            _isReadyToPush = false;
        }
    }
}