using System;
using UnityEngine;

public class WoodCutter : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem))
        {
            col.gameObject.SetActive(false);
        }
    }
}
