    using System;
    using UnityEngine;

    public class DollCollideWater : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.layer == 4)
            {
                print("collide water");
                Collider[] colliders = gameObject.GetComponents<Collider>();
                foreach (var collider in colliders)
                {
                    collider.isTrigger = false;
                }
            }
        }
    }
