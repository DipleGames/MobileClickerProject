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

    /// <summary>
    /// 새 저장 데이터를 임시 파일에 기록한 뒤 본 파일로 교체한다.
    /// </summary>
    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        string temporaryPath = savePath + ".tmp";
        string backupPath = savePath + ".bak";
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        using (FileStream stream = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (StreamWriter writer = new StreamWriter(stream, new System.Text.UTF8Encoding(false)))
        {
            writer.Write(json);
            writer.Flush();
            stream.Flush(true);
        }

        if (File.Exists(savePath))
        {
            ReplaceSaveFile(temporaryPath, backupPath);
        }
        else
        {
            File.Move(temporaryPath, savePath);
        }

        Debug.Log($"저장완료 {savePath}");
    }

    /// <summary>
    /// 임시 파일을 본 저장 파일로 교체하고 기존 파일을 백업한다.
    /// </summary>
    private void ReplaceSaveFile(string temporaryPath, string backupPath)
    {
        try
        {
            File.Replace(temporaryPath, savePath, backupPath);
        }
        catch (PlatformNotSupportedException)
        {
            ReplaceSaveFileWithBackup(temporaryPath, backupPath);
        }
        catch (NotSupportedException)
        {
            ReplaceSaveFileWithBackup(temporaryPath, backupPath);
        }
        catch (IOException)
        {
            ReplaceSaveFileWithBackup(temporaryPath, backupPath);
        }
    }

    /// <summary>
    /// 파일 교체 기능을 지원하지 않는 환경에서 백업을 보존하며 저장 파일을 교체한다.
    /// </summary>
    private void ReplaceSaveFileWithBackup(string temporaryPath, string backupPath)
    {
        File.Copy(savePath, backupPath, true);
        File.Delete(savePath);
        File.Move(temporaryPath, savePath);
    }

    /// <summary>
    /// 저장 파일이 없거나 손상된 경우 백업 또는 완성된 임시 파일에서 복구한다.
    /// </summary>
    public void RecoverSaveFile()
    {
        string temporaryPath = savePath + ".tmp";
        string backupPath = savePath + ".bak";
        string corruptPath = savePath + ".corrupt";

        if (IsValidSaveFile(savePath))
        {
            return;
        }

        if (IsValidSaveFile(temporaryPath))
        {
            File.Copy(temporaryPath, savePath, true);
            Debug.LogWarning($"저장 파일이 없어 임시 저장 파일에서 복구했습니다: {savePath}");
            return;
        }

        if (IsValidSaveFile(backupPath))
        {
            File.Copy(backupPath, savePath, true);
            Debug.LogWarning($"저장 파일이 손상되어 백업에서 복구했습니다: {savePath}");
            return;
        }

        if (File.Exists(savePath))
        {
            File.Copy(savePath, corruptPath, true);
            File.Delete(savePath);
            Debug.LogError($"복구할 수 없는 저장 파일을 보존했습니다: {corruptPath}");
        }
    }

    /// <summary>
    /// 파일을 읽고 저장 데이터로 역직렬화할 수 있는지 확인한다.
    /// </summary>
    private bool IsValidSaveFile(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            string json = File.ReadAllText(path);
            string trimmedJson = json.Trim();
            if (trimmedJson.Length < 2 || trimmedJson[0] != '{' || trimmedJson[trimmedJson.Length - 1] != '}')
            {
                return false;
            }

            return JsonUtility.FromJson<SaveData>(trimmedJson) != null;
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"저장 파일을 읽을 수 없습니다: {path} ({exception.Message})");
            return false;
        }
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
    /// 본 저장 파일과 복구에 사용되는 임시 파일 및 백업을 삭제한다.
    /// </summary>
    public void DeleteSave()
    {
        string[] savePaths = { savePath, savePath + ".tmp", savePath + ".bak" };
        foreach (string path in savePaths)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        Debug.Log("저장 데이터를 삭제했습니다.");
    }

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }
}
