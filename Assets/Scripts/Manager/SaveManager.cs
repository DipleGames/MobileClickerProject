using System.IO;
using UnityEngine;

public class SaveManager : SingleTon<SaveManager>
{
    public string _savePath;

    protected override void Awake()
    {
        base.Awake();

        _savePath = Path.Combine(Application.persistentDataPath,"save.json");
    }
    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(_savePath, json);

        Debug.Log($"저장완료 {_savePath}");
    }

    public SaveData Load()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("저장 파일이 없습니다.");
            return new SaveData();
        }

        string json = File.ReadAllText(_savePath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        return saveData;
    }

    public void DeleteSave()
    {
        if (!File.Exists(_savePath))
            return;

        File.Delete(_savePath);

        Debug.Log("저장 데이터를 삭제했습니다.");
    }

    public bool HasSaveData()
    {
        return File.Exists(_savePath);
    }
}
