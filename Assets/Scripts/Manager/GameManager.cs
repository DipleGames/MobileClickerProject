using System;
using UnityEngine;
using System.IO;
using System.Collections;

public class GameManager : SingleTon<GameManager>
{
    private SaveData _saveData;
    private Coroutine _gameCoroutine;

    void Start()
    {
        SetGame();
    }

    void SetGame()
    {
        _saveData = LoadData(); // 데이터를 로드하고
        ScoreManager.Instance.CurrentScore = _saveData.currentScore;
        HUDManager.Instance.UpdateScoreUI(_saveData.currentScore);
        _gameCoroutine = StartCoroutine(StartGameRoutine());
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
            DateTime lastSaveTime = DateTime.Parse(saveData.lastSaveTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            TimeSpan elapsed = DateTime.UtcNow - lastSaveTime;

            double offlineSeconds = elapsed.TotalSeconds;

            long reward = (long)(offlineSeconds);

            Debug.Log($"오프라인 시간 : {elapsed.TotalSeconds}초, 리워드 : {reward}");
            ScoreManager.Instance.GiveOfflineReward(saveData, reward);
        }

        return saveData;

    }


    IEnumerator StartGameRoutine()
    {
        int stage = 1;
        Debug.Log("루틴");
        while(true)
        {
            StageManager.Instance.ChangeStage(stage);
            yield return new WaitUntil(() => StageManager.Instance.IsClear);
            stage = StageManager.Instance.CurrentStage + 1;
        }
    }

    private void StopGameRoutine()
    {
        if (_gameCoroutine != null)
        {
            StopCoroutine(_gameCoroutine);
            _gameCoroutine = null;
        }
    }
}
