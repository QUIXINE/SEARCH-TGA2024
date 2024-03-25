using UnityEngine.SceneManagement;
using UnityEngine;

    public class SpawnTest : MonoBehaviour
    {
        public void SpawnDamage(Scene sceneToLoad)
        {
            Debug.Log(sceneToLoad.name);
        }
    }
