using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuOption : MonoBehaviour
{
    [SerializeField] private GameObject _newGameBtn;
    [SerializeField] private SaveAndLoadLevel _saveAndLoadLevel;
    private void Awake()
    {
        PlayerPrefs.GetInt("IsPlayerPressed");
        if (PlayerPrefs.GetInt("IsPlayerPressed") == 1)
        {
            // show UI NewGame btn
            _newGameBtn.SetActive(true);
        }
    }

    public void Play()
    {
        if (_saveAndLoadLevel.LevelDataInfo.PlayerSceneName == "PlayerClownHouse")
        {
            SceneManager.LoadScene(_saveAndLoadLevel.LevelDataInfo.PlayerSceneName);
        }
        else
        {
            // Load PlayerForest
            SceneManager.LoadScene(_saveAndLoadLevel.LevelDataInfo.PlayerSceneName);
        }

        if (PlayerPrefs.GetInt("IsPlayerPressed") == 0)
        {
            PlayerPrefs.SetInt("IsPlayerPressed", 1);
        }
    }

    public void NewGame()
    {
        print("New Game");
        _saveAndLoadLevel.LevelDataInfo.SpawnPointX = 0;
        _saveAndLoadLevel.LevelDataInfo.SpawnPointY = 0;
        _saveAndLoadLevel.LevelDataInfo.SpawnPointZ = 0;
        _saveAndLoadLevel.LevelDataInfo.CurrentScene = "ForestMech1";
        _saveAndLoadLevel.LevelDataInfo.PlayerSceneName = "PlayerForest";
        _saveAndLoadLevel.SaveLevelData();
        SceneManager.LoadScene(_saveAndLoadLevel.LevelDataInfo.PlayerSceneName);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
