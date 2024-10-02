using System;
using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] private GameObject _optionsUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_optionsUI.activeInHierarchy)
        {
            _optionsUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _optionsUI.activeInHierarchy)
        {
            _optionsUI.SetActive(false);
        }
    }

    public void Resume()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
