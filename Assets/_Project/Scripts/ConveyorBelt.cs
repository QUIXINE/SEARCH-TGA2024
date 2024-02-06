using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Move The Wood")]
    public bool IsMoving;
    [SerializeField] private List<GameObject> woods = new List<GameObject>();
    [FormerlySerializedAs("_moveSpeed")] [SerializeField] private float _woodMoveSpeed;
    
    [Header("Move Player")]
    [SerializeField] private float _playerMoveSpeed;
    private GameObject _player;
    private bool _isPlayerOn;
    
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.TryGetComponent<MovableObjectCheckMoving>(out var movableObj) &&
            col.gameObject.CompareTag(TagManager.MovableItem)
            && woods != null)
        {
            /*Vector3 move = col.gameObject.transform.right * _moveSpeed * Time.deltaTime;
            rb.MovePosition(col.gameObject.transform.position + move);*/

            //Problem: it assigns 1 obj to all 10, and if there are more than 1 obj, only 1 obj got assign to array
            //Problem: clown house puzzle 4, IsMoving = false while wood is on conveyor belt
            //print(col.gameObject.name);
            
            //Problem: The code isn't "Assign" the parent
            /*for (int i = 0; i <= woods.Count; i++)
            {
                if (i == 0)
                {
                    print("i = 0");

                    if (woods == null && col.gameObject.transform.parent)
                    {
                        print("Assign Parent");
                        woods.Add(col.gameObject.transform.parent.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                        // movableObj.IsMoving = true;
                    }
                    else if (woods == null)
                    {
                        print("Assign");
                        //print("i == 0");
                        woods.Add(col.gameObject.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                }
                else if (i != 0)
                {
                    if (woods == null && col.gameObject.transform.parent &&
                        col.gameObject.transform.parent.gameObject != woods[i - 1]
                        && woods.All(l => l != col.gameObject))
                    {
                        print("Assign Parent");
                        woods.Add(col.gameObject.transform.parent.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                    else if (woods == null && col.gameObject != woods[i - 1] && !col.gameObject.transform.parent &&
                             woods.All(l => l != col.gameObject))
                    {
                        //print("i != 0");
                        print("Assign");
                        woods.Add(col.gameObject.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                }
            }*/

            if (IsMoving)
            {
                print("IsMoving");
                for (int i = 0; i < woods.Count; i++)
                {
                    if (woods[i] != null)
                    {
                        if (woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving)
                        {
                            woods[i].transform.position += Vector3.right * _woodMoveSpeed * Time.deltaTime;
                        }
                    }
                    /*woods[i].GetComponent<Rigidbody>().MovePosition(woods[i].transform.position +
                                                                    (woods[i].transform.right * _moveSpeed * Time.deltaTime));*/
                }
            }
        }
        else if (_isPlayerOn && IsMoving)
        {
            print("Player Move");
            //Move player, gameObj local transform now is forward it's why I set position + transform.forward instead of transform.right
            //but if gameObj local transform is right, then set to transform.right
            _player.transform.position += transform.right * _playerMoveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.TryGetComponent<MovableObjectCheckMoving>(out var movableObj) &&
            col.gameObject.CompareTag(TagManager.MovableItem))
        {
            for (int i = 0; i <= woods.Count; i++)
            {
                if(i == 0)
                {
                    print("i = 0");

                    if (woods.Count == 0 && col.transform.parent && col.transform.parent.root.gameObject != _player)
                    {
                        print("Assign Parent");
                        woods.Add(col.gameObject.transform.parent.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                    else if (woods.Count == 0 && !col.transform.parent)
                    {
                        print("Assign");
                        //print("i == 0");
                        woods.Add(col.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                }
                else if (i <= 10)
                {
                    if (woods.Count > 0 && col.transform.parent && col.transform.parent.root.gameObject != _player && woods.All(l => l != col.transform.parent.gameObject))
                    {
                        print("Assign Parent");
                        woods.Add(col.gameObject.transform.parent.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                    else if (woods.Count > 0 && !col.transform.parent && woods.All(l => l != col.gameObject))
                    {
                        //print("i != 0");
                        print("Assign");
                        woods.Add(col.gameObject.gameObject);
                        woods[i].GetComponent<MovableObjectCheckMoving>().IsMoving = true;
                    }
                }
            }
            
            //This code adds Box (in WoodShaper)
            /*movableObj.IsMoving = true;
            woods.Add(col.gameObject);*/
        }


        if (col.gameObject.CompareTag(TagManager.Player))
        {
            _player = col.gameObject;
            _isPlayerOn = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.TryGetComponent<MovableObjectCheckMoving>(out var movableObj) &&
            col.gameObject.CompareTag(TagManager.MovableItem))
        {
            movableObj.IsMoving = false;
            woods.Remove(col.gameObject);
        }
        else if (col.gameObject.CompareTag(TagManager.Player))
        {
            _player = null;
            _isPlayerOn = false;
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
