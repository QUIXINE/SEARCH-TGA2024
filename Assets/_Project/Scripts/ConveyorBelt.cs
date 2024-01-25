using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ConveyorBelt : MonoBehaviour
{
    public bool IsMoving;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private List<GameObject> woods = new List<GameObject>(); 

    private void Update()
    {
        if (IsMoving && woods != null)
        {
            for (int i = 0; i < woods.Count; i++)
            {
                if(woods[i] != null)
                {
                    if (woods[i].GetComponent<MovableIObjectCheckMoving>().IsMoving)
                    {
                        woods[i].transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
                    }
                }
                /*woods[i].GetComponent<Rigidbody>().MovePosition(woods[i].transform.position + 
                                                                (woods[i].transform.right * _moveSpeed * Time.deltaTime));*/
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.TryGetComponent<MovableIObjectCheckMoving>(out var movableObj) && col.gameObject.CompareTag(TagManager.MovableItem))
        {
            /*Vector3 move = col.gameObject.transform.right * _moveSpeed * Time.deltaTime;
            rb.MovePosition(col.gameObject.transform.position + move);*/
            
                //Problem: it assigns 1 obj to all 10, and if there are more than 1 obj, only 1 obj got assign to array
                //Problem: clown house puzzle 4, IsMoving = false while wood is on conveyor belt
                print(col.gameObject.name);
                for (int i = 0; i < woods.Count; i++)
                {
                    if(i == 0)
                    {
                        if (woods[i] == null && col.gameObject.transform.parent)
                        {
                            woods[i] = col.gameObject.transform.parent.gameObject;
                            movableObj.IsMoving = true;
                        }
                        else if (woods[i] == null)
                        {
                            print("i == 0");
                            woods[i] = col.gameObject;
                            movableObj.IsMoving = true;
                        }
                    }
                    else if (i != 0)
                    {
                        if (woods[i] == null && col.gameObject.transform.parent && col.gameObject.transform.parent.gameObject != woods[i-1]
                            && woods.All(l => l != col.gameObject))
                        {
                            woods[i] = col.gameObject.transform.parent.gameObject;
                            movableObj.IsMoving = true;
                        }
                        else if (woods[i] == null && col.gameObject != woods[i-1] && !col.gameObject.transform.parent && woods.All(l => l != col.gameObject))
                        {
                            print("i != 0");
                            woods[i] = col.gameObject;
                            movableObj.IsMoving = true;
                        }
                    }
                }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.TryGetComponent<MovableIObjectCheckMoving>(out var movableObj) &&
            col.gameObject.CompareTag(TagManager.MovableItem))
        {
            movableObj.IsMoving = false;
        }
        //IsMoving = false; //can't use this because it'll stop all obj, use stopping each obj instead
    }
}

/*puzzle 5 works with this code
 for (int i = 0; i < woods.Length; i++)
{
        if (woods[i] == null && col.gameObject.transform.parent)
        {
            woods[i] = col.gameObject.transform.parent.gameObject;
        }
}*/
