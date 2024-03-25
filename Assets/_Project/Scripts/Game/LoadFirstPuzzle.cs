using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFirstPuzzle : MonoBehaviour
{
    [SerializeField] private SceneField _scenesToLoad;
    private void Awake()
    {
        SceneManager.LoadSceneAsync(_scenesToLoad, LoadSceneMode.Additive);
    }
}
