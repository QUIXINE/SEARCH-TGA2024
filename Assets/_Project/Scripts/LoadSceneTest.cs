using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTest : MonoBehaviour
{
    [SerializeField] private SceneField[] _sceneToLoad;
    private void Start()
    {
        foreach (var scene in _sceneToLoad)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            
        }

    }
}
