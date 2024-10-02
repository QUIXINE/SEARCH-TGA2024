using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkipMapScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Canvas;
    public TextMeshProUGUI TextCurrentNumber;
    public int CurrentList = 0;
    public List<Transform> WarpPoints;
    private void Awake()
    {
        Canvas.SetActive(false);
    }
    private void Update()
    {
        CheatOn();
        if (Canvas == true)
        {
            NextWayPoint();
            GoOn();
            PreviousWayPoint();
            TextCurrentNumber.text = "Number of Points: " + CurrentList;
        }
        StartPlaying();
    }
    private void CheatOn()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Canvas.SetActive(true);
        }
    }
    private void NextWayPoint()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {

            if(CurrentList < WarpPoints.Count)
            {
                if (CurrentList <= WarpPoints.Count)
                {
                    CurrentList += 1;
                }
            }
        }
    }
    private void PreviousWayPoint()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {

            if (CurrentList < WarpPoints.Count)
            {
                if (CurrentList <= WarpPoints.Count)
                {
                    CurrentList -= 1;
                }
            }
        }
    }
    private void GoOn()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Player.transform.position = WarpPoints[CurrentList].position;
            Player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void StartPlaying()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Player.GetComponent<PlayerController>().enabled = true;
            Player.GetComponent<Rigidbody>().isKinematic = false;
            Player.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
