using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelMetaData
{
    public string levelId;
    public string sceneName;
    public float timeLimit;
    public int numCollectibles;
    public int scoreRequirement;
}

[System.Serializable]
public class ScoreData
{
    public string numCollected = "?/?";
    public int score;

    public ScoreData(string numCollected, int score)
    {
        this.numCollected = numCollected;
        this.score = score;
    }
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }


    public List<LevelMetaData> levelData;
    private Dictionary<string, LevelMetaData> dataMap;
    
    public LevelMetaData currentLevel;
    public bool currentIsPractice;

    public Dictionary<string, ScoreData> scoreMap = new Dictionary<string, ScoreData>();
    public string scoreDataPath;


    private void Awake()
    {


        // Ensure only one instance exists (Singleton pattern)
        if (Instance == null)
        {

            dataMap = new Dictionary<string, LevelMetaData>();
            foreach (LevelMetaData level in levelData)
            {
                dataMap[level.levelId] = level;
            }

            Instance = this;
            scoreDataPath = Path.Combine(Application.persistentDataPath, "levelData.json");
            loadScores();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    public LevelMetaData getMetaData(string levelId)
    {
        if(dataMap.ContainsKey(levelId))
        {
            return dataMap[levelId];
        };
        return null;
    }

    public ScoreData getScores(string levelId)
    {
        if(scoreMap.ContainsKey(levelId))
        {
            return scoreMap[levelId];
        }
        return null;
    }



    public void saveScores()
    {
        string jsonData = JsonUtility.ToJson(new SerializationWrapper<ScoreData>(scoreMap), true);
        File.WriteAllText(scoreDataPath, jsonData);
        Debug.Log("Level data saved to " + scoreDataPath);
    }


    public void loadScores()
    {
        if (File.Exists(scoreDataPath))
        {
            string jsonData = File.ReadAllText(scoreDataPath);
            scoreMap = JsonUtility.FromJson<SerializationWrapper<ScoreData>>(jsonData).ToDictionary();
            Debug.Log("Level data loaded from " + scoreDataPath);
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }
    }

    //public LevelMetaData GetLevelData(string levelName)
    //{
    //    if (levelDataDictionary.TryGetValue(levelName, out LevelMetaData levelData))
    //    {
    //        return levelData;
    //    }
    //    Debug.LogWarning("Level data not found for level: " + levelName);
    //    return new LevelMetaData();
    //}


    // Additional methods for managing level data can be added here
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<string> keys;
    public List<T> values;

    public SerializationWrapper(Dictionary<string, T> dictionary)
    {
        keys = new List<string>(dictionary.Keys);
        values = new List<T>(dictionary.Values);
    }

    public Dictionary<string, T> ToDictionary()
    {
        Dictionary<string, T> result = new Dictionary<string, T>();
        for (int i = 0; i < keys.Count; i++)
        {
            result[keys[i]] = values[i];
        }
        return result;
    }
}