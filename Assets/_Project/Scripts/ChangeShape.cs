using System;
using UnityEngine;

public class ChangeShape : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(TagManager.MovableItem))
        {
            Transform parent = col.gameObject.transform.parent;
            Transform dollBody = parent.GetChild(1);
            print(dollBody.gameObject);
            if (dollBody.gameObject.name == "Doll Body" && col.gameObject.name == "Box")
            {
                print("Change");
                dollBody.gameObject.SetActive(true);
                col.gameObject.SetActive(false);
            }
            
            /*int childsCount = parent.childCount;
            GameObject[] childs = new GameObject[] {};
            for (int i = 0; i < childsCount; i++)
            {
                childs[i] =
            }*/
        }
        else if (col.gameObject.CompareTag(TagManager.Player))
        {
            PlayerTakeDamage playerTakeDamage = col.gameObject.GetComponent<PlayerTakeDamage>();
            playerTakeDamage.TakeDamage();
        }
    }
}
