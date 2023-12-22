using UnityEngine;

public class Player_Push_Button : MonoBehaviour 
{
    private bool isReadyToPush;
    private IButton button;

    private void Update() 
    {
        if(isReadyToPush && Input.GetKeyDown(KeyCode.RightControl))
        {
            button.Execute();
        }    
    }
    private void OnTriggerEnter(Collider col) 
    {
        if(col.gameObject.CompareTag(Tag_Manager.Button))
        {
            print("Collide");
            button = col.gameObject.GetComponent<IButton>();
            isReadyToPush = true;
        }
    }    

    private void OnTriggerExit(Collider col) 
    {
        if(col.gameObject.CompareTag(Tag_Manager.Button))
        {
            print("Exit");
            button = null;
            isReadyToPush = false;
        }
    }
}