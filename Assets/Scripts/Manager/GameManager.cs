using System;
using UnityEngine;
using System.IO;

public class GameManager : SingleTon<GameManager>
{
    SaveData saveData;
    void Start()
    {
        SetGame();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void SetGame()
    {
        saveData = LoadData(); // 데이터를 로드하고
        ScoreManager.Instance.CurrentScore = saveData.currentScore;
        HUDManager.Instance.UpdateScoreUI(saveData.currentScore);
    }

    public SaveData LoadData()
    {
        if (!File.Exists(SaveManager.Instance.savePath))
        {
            Debug.Log("저장 파일이 없습니다.");
            return new SaveData();
        }

        string json = File.ReadAllText(SaveManager.Instance.savePath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        if (!string.IsNullOrEmpty(saveData.lastSaveTime))
        {
            DateTime lastTime = DateTime.Parse(saveData.lastSaveTime);
            TimeSpan elapsed = DateTime.UtcNow - lastTime;

            Debug.Log($"오프라인 시간 : {elapsed.TotalSeconds}초");
        }

        return saveData;

    }


    public void SaveData()
    {
        SaveData saveData = new SaveData()
        {
            currentScore = ScoreManager.Instance.CurrentScore,
            lastSaveTime = DateTime.UtcNow.ToString("O")
        };
        
        SaveManager.Instance.Save(saveData);
    }
}
