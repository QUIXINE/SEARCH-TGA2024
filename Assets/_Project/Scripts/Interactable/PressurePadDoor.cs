using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ClownHPuzzle6
{
    public class PressurePadDoor : MonoBehaviour
    {
        public TextMeshProUGUI PeopleAmountText;
        public int ObjsCount;
        public bool IsDoorMoving;
        
        [Header("Activate Secret Door")] [SerializeField]
        private GameObject _secretDoor;

        [SerializeField] private float _getUpSpeed;

        [Tooltip("manually set y-axis to tell where Secret Door need to go)")] [SerializeField]
        private float _yPos;


        private GameObject[] _dolls;
        private Vector3 _endPosition, _targetPosition;

        private void Start()
        {
            _endPosition = new Vector3(_secretDoor.transform.localPosition.x, _yPos,
                _secretDoor.transform.localPosition.z);
            _targetPosition = _endPosition;
        }

        private void Update()
        {
            if (ObjsCount == 3)
            {
                IsDoorMoving = true;
                /*foreach (var doll in _dolls)
            {
                doll.GetComponent<Collider>().enabled = false;
            }*/
                if (_targetPosition == _endPosition)
                {
                    _secretDoor.transform.localPosition =
                        Vector3.MoveTowards(_secretDoor.transform.localPosition, _targetPosition,
                            _getUpSpeed * Time.deltaTime);
                    if (Vector3.Distance(_secretDoor.transform.position, _targetPosition) <= 0.001f)
                    {
                        ObjsCount = 0;
                    }
                }
            }
        }
        
        
        private void OnCollisionEnter(Collision col)
        {
            if (!IsDoorMoving)
            {
                if (col.gameObject.CompareTag(TagManager.Player))
                {
                    ObjsCount++;
                    PeopleAmountText.text = ObjsCount.ToString();
                }
            }
        }
        
        private void OnCollisionExit(Collision col)
        {
            if (!IsDoorMoving)
            {
                if (col.gameObject.CompareTag(TagManager.Player))
                {
                    ObjsCount--;
                    PeopleAmountText.text = ObjsCount.ToString();
                }
            }
        }
    }

}