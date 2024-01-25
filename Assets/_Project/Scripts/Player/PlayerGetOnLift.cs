using UnityEngine;

public class PlayerGetOnLift : MonoBehaviour 
{
    private void OnCollisionEnter(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = col.gameObject.transform.parent;
        }    
    }

    private void OnCollisionExit(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = null;
        }    
    }
}