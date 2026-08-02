using System;
using System.IO;
using UnityEngine;

public class SaveManager : SingleTon<SaveManager>
{
    public string savePath;

    protected override void Awake()
    {
        base.Awake();

        savePath = Path.Combine(Application.persistentDataPath,"save.json");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);

        Debug.Log($"저장완료 {savePath}");
    }

    public void SaveData()
    {
        SaveData saveData = new SaveData()
        {
            currentScore = ScoreManager.Instance.CurrentScore,
            lastSaveTime = DateTime.UtcNow.ToString("O")
        };
        
        Save(saveData);
    }


    public void DeleteSave()
    {
        if (!File.Exists(savePath))
            return;

        File.Delete(savePath);

        Debug.Log("저장 데이터를 삭제했습니다.");
    }

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }
}
