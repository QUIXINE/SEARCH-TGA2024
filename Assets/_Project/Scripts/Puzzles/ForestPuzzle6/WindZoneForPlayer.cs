using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Puzzles.WindZone
{
    public class WindZoneForPlayer : MonoBehaviour
    {
        [SerializeField] private float _floatAngle;
        [SerializeField] private float _windForce;
        private GameObject _player;
        private Rigidbody _rb;
        private bool _IsBlowing;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>().gameObject;
            _rb = _player.GetComponent<Rigidbody>();
        }

        /*private void Update()
        {
            if (_IsBlowing)
            {
                // _rb.AddForce((Quaternion.Euler(_floatAngle,0,0) * _player.transform.forward) * _windForce * Time.deltaTime);
                // _player.transform.position += Vector3.back * _windForce;
                _player.transform.position += (Quaternion.Euler(_floatAngle,0,0) * Vector3.back) * _windForce * Time.deltaTime;
                
            }
        }*/

        private void OnTriggerStay(Collider col)
        {
            if (col.gameObject.CompareTag(TagManager.Player))
            {
                print("Blow Player");
                var hitObj = col.gameObject;
                var rb = hitObj.GetComponent<Rigidbody>();
                var dir = -Vector3.forward;
                Vector3 rotated = Quaternion.Euler(0, 0, _floatAngle) * Vector3.one;
                float angleOfMovement = Mathf.Atan2(rotated.x, rotated.z) * Mathf.Rad2Deg;
                Quaternion currentRotation = hitObj.transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angleOfMovement);
                // hitObj.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, 1 * Time.deltaTime);
                // rb.velocity += Vector3.right * _windForce;
                _player.transform.position += (Quaternion.Euler(_floatAngle,0,0) * Vector3.back) * _windForce * Time.deltaTime;
                // rb.AddForce((Quaternion.Euler(_floatAngle,0,0) * Vector3.back) * _windForce * Time.deltaTime);
                _IsBlowing = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.gameObject.CompareTag(TagManager.Player))
            {
                col.GetComponent<PlayerTakeDamage>().TakeDamage(gameObject.scene);
            }
        }
    }
}