using System.Collections.Generic;
using System.IO;
using System.Linq;
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
public class ScoreData : System.IComparable<ScoreData>
{
    public string numCollected = "?/?";
    public int score;
    public float totalTime;

    public ScoreData(string numCollected, int score, float totalTime)
    {
        this.numCollected = numCollected;
        this.score = score;
        this.totalTime = totalTime;
    }
    
    public int CompareTo(ScoreData other)
    {
        return this.totalTime.CompareTo(other.totalTime);
    }
}


// Wrapper for serialization
[System.Serializable]
public class ScoreListWrapper
{
    public List<ScoreData> scoreData;

    public ScoreListWrapper(List<ScoreData> scoreData)
    {
        this.scoreData = scoreData;
    }
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }


    public List<LevelMetaData> levelData;
    private Dictionary<string, LevelMetaData> dataMap;
    
    public LevelMetaData currentLevel;
    public bool currentIsPractice;

    public Dictionary<string, List<ScoreData>> scoreMap = new Dictionary<string, List<ScoreData>>();
    public string scoreDataPath;


    private void Awake()
    {
        // Ensure only one instance exists (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
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

        dataMap = new Dictionary<string, LevelMetaData>();
        foreach (LevelMetaData level in levelData)
        {
            dataMap[level.levelId] = level;
        }
    }

    public string getLevelSavePath(string levelId)
    {
        return Path.Combine(Application.persistentDataPath, levelId);
    }

    public LevelMetaData getMetaData(string levelId)
    {
        if(dataMap.ContainsKey(levelId))
        {
            return dataMap[levelId];
        };
        return null;
    }

    public List<ScoreData> getScores(string levelId)
    {
        if(scoreMap.ContainsKey(levelId))
        {
            return scoreMap[levelId];
        }
        return null;
    }

    public void saveScore(ScoreData data, string levelId)
    {
        
        List<ScoreData> scores;
        if(scoreMap.ContainsKey(levelId))
        {
            scores = scoreMap[levelId];
        }
        else
        {
            scores = new List<ScoreData>();
        }
        
        scores.Add(data);
        scores.Sort();
        
        // Keeping top 5 scores only
        if (scores.Count > 5)
        {
            scores = scores.Take(5).ToList();
        }
        
        scoreMap[levelId] = scores;
        string jsonData = JsonUtility.ToJson(new ScoreListWrapper(scores), true);
        File.WriteAllText(getLevelSavePath(levelId), jsonData);

    }


    public void loadScores()
    {

        foreach(LevelMetaData level in levelData)
        {
            string levelSavePath = getLevelSavePath(level.levelId);

            Debug.Log(level.levelId);
            if (File.Exists(levelSavePath))
            {
                string jsonData = File.ReadAllText(levelSavePath);
                ScoreListWrapper levelScores = JsonUtility.FromJson<ScoreListWrapper>(jsonData);
                scoreMap[level.levelId] = levelScores.scoreData;
            }
            else
            {
                Debug.LogWarning($"Save file not found for {level.levelId}");
            }
        }
    }
}