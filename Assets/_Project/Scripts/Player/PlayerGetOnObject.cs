using UnityEngine;

public class PlayerGetOnObject : MonoBehaviour 
{
    private void OnCollisionEnter(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = col.gameObject.transform.parent;
        }
        else if(col.gameObject.CompareTag(TagManager.MovableItem))
        {
            transform.parent = col.gameObject.transform;
        }
        else if(col.gameObject.CompareTag(TagManager.Crusher))
        {
            transform.parent = col.gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision col) 
    {
        if(col.gameObject.CompareTag(TagManager.Lift))
        {
            transform.parent = null;
        }   
        else if(col.gameObject.CompareTag(TagManager.MovableItem))
        {
            transform.parent = null;
        }
        else if(col.gameObject.CompareTag(TagManager.Crusher))
        {
            transform.parent = null;
        }
    }
}