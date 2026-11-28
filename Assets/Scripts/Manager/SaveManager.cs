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

    /// <summary>
    /// 게임 종료 시 진행 상태를 저장한다.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveData();
    }

    /// <summary>
    /// 앱이 백그라운드로 전환될 때 진행 상태를 저장한다.
    /// </summary>
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            SaveData();
        }
    }

    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);

        Debug.Log($"저장완료 {savePath}");
    }

    /// <summary>
    /// 초기화가 끝난 게임 상태를 저장 파일에 기록한다.
    /// </summary>
    public void SaveData()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameInitialized)
        {
            return;
        }

        SaveData saveData = new SaveData()
        {
            currentStage = StageManager.Instance.CurrentStage,
            currentScore = ScoreManager.Instance.CurrentScore,
            currentAttackLevel = AttackManager.Instance.AttackLevel,
            currentAbilityLevel = AbilityManager.Instance.AbilityLevel,
            currentDropLevel = DropManager.Instance.DropLevel,
            lastSaveTime = DateTime.UtcNow.ToString("O"),
            skillDatas = SkillManager.Instance.GetSaveSkillDatas(),
            equippedSkillIDs = SkillManagement.Instance.GetEquippedSkillIDs()
        };
        
        Save(saveData);
    }


    /// <summary>
    /// 저장 파일이 있으면 삭제한다.
    /// </summary>
    public void DeleteSave()
    {
        if (!File.Exists(savePath))
        {
            return;
        }

        File.Delete(savePath);

        Debug.Log("저장 데이터를 삭제했습니다.");
    }

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }
}
