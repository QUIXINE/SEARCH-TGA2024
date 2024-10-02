using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxesLoop : MonoBehaviour
{
    public bool IsBoxPassedBridge;
    public List<GameObject> Boxes => _boxes;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _boxes;
    [SerializeField] private ReleaserDoor _releaserDoor;
    [SerializeField] private float _releaseWaitTime;   //time used to move wood to pos inside the releaser 1 by 1, used to
    private float _releaseWaitTimeDecreased;  //time used to move wood to pos inside the releaser 1 by 1, used to
    [SerializeField] private float _spawningDelay;

    private bool _areBoxesReleasing;

    //wait for making a space between wood
    private void Start()
    {
        _releaseWaitTimeDecreased = _releaseWaitTime;
    }

    private void Update()
    {

        if (_boxes.All(x => x.transform.position == _spawnPoint.position) && _releaseWaitTimeDecreased > 0 && !IsBoxPassedBridge)
        {
            print("Decrease Time");
            _releaseWaitTimeDecreased -= Time.deltaTime;
        }
        // check if there's no any boxes pass to another conveyor belt as well
        else if(_boxes.All(x => x.transform.position == _spawnPoint.position) && _releaseWaitTimeDecreased <= 0 
                && !_releaserDoor.IsDoorOpening && !_releaserDoor.IsDoorOpened)
        {
            print("Open Door");
            // Open releaser's door
            _releaserDoor.IsDoorOpening = true;
            
            // assign bool that door is opening. Use the bool ion IEnumerator
            // Assign IsDoorOpening to false, IsDoorOpened to true
            StartCoroutine(_releaserDoor.OpenDoor());
        }
        else if (_boxes.All(x => x.transform.position == _spawnPoint.position) && _releaseWaitTimeDecreased <= 0 
                && _releaserDoor.IsDoorOpened && !_areBoxesReleasing)
        {
            print("Release Boxes");
            _areBoxesReleasing = true;
            StartCoroutine(ReleaseBoxes());
        }
        
   
    }


    private IEnumerator ReleaseBoxes()
    {
        while (_areBoxesReleasing)
        {
            /*if (!IsBoxPassedBridge)
            {*/
            yield return new WaitForSecondsRealtime(_spawningDelay);

                if (!_boxes[0].activeSelf)
                {
                    print("release _boxes[0]");
                    _boxes[0].SetActive(true);
                }
                else if (!_boxes[1].activeSelf)
                {
                    print("release _boxes[1]");
                    _boxes[1].SetActive(true);
                }
                else if (!_boxes[2].activeSelf)
                {
                    print("release _boxes[2]");
                    _boxes[2].SetActive(true);
                }
            // }
            if (_boxes.All(x => x.activeSelf))
            {
                _areBoxesReleasing = false;
                _releaseWaitTimeDecreased = _releaseWaitTime;
            }
            
        }
    }
    
    public void ReturnToPool(GameObject box)
    {
        print("ReturnToPool");
        box.SetActive(false);
        box.transform.position = _spawnPoint.position;
    }
}