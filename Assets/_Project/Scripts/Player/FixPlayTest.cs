using System;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class FixPlayTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position += transform.rotation.y > 0? Vector3.right : Vector3.left;
            }
            
        }
    }
}