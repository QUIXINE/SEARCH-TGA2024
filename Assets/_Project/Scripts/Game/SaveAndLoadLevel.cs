using System.IO;
using UnityEngine;

public sealed class SaveAndLoadLevel : MonoBehaviour
{
    private static readonly string s_originalPath = "Assets/_Project/Resources/LevelData.txt";
    private static string s_newPath;

    public LevelData LevelDataInfo;
    
    private void Awake() 
    {
        s_newPath = Path.Combine(Application.persistentDataPath, "LevelData.json");
        LoadLevelData();    
    }

    private void LoadLevelData()
    {
        LevelData levelData = null;
        if(File.Exists(s_newPath))
        {
            print("access local file");
            string jsonFile = File.ReadAllText(s_newPath);
            levelData = JsonUtility.FromJson<LevelData>(jsonFile);
        }
        else if(File.Exists(s_originalPath) && !File.Exists(s_newPath))
        {
            print("access asset file");
            var json = Resources.Load<TextAsset>("LevelData");
            levelData = JsonUtility.FromJson<LevelData>(json.text);
        }
        else
        {
            Debug.LogWarning("File not found: " + s_originalPath);
        }
        LevelDataInfo = levelData;
    }
    
    public void SaveLevelData()
    {
        LevelData levelData = LevelDataInfo;
        string json = JsonUtility.ToJson(levelData);
        File.WriteAllText(s_newPath, json);
    }
}
