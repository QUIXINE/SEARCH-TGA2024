using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GetInsideHouseTrigger : MonoBehaviour
{
    [SerializeField] private SaveAndLoadLevel _saveAndLoadLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.Player))
        {
            _saveAndLoadLevel = FindObjectOfType<SaveAndLoadLevel>();
            _saveAndLoadLevel.LevelDataInfo.PlayerSceneName = "PlayerClownHouse";
            _saveAndLoadLevel.LevelDataInfo.CurrentScene = "ClownHMech1 Original";
            _saveAndLoadLevel.LevelDataInfo.SpawnPointX = 0;
            _saveAndLoadLevel.LevelDataInfo.SpawnPointY = 0;
            _saveAndLoadLevel.LevelDataInfo.SpawnPointZ = 0;
            _saveAndLoadLevel.SaveLevelData();
            SceneManager.LoadScene(_saveAndLoadLevel.LevelDataInfo.PlayerSceneName);
        }
    }
}
