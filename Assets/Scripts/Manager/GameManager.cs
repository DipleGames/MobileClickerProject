using System;
using UnityEngine;
using System.IO;
using System.Collections;

public class GameManager : SingleTon<GameManager>
{
    private SaveData _saveData;
    private Coroutine _gameCoroutine;
    [SerializeField] private OfflineRewardPopup _offlineRewardPopup; // 게임 시작 시 오프라인 보상을 표시할 팝업
    private long _offlineReward; // 이번 접속에서 지급한 오프라인 골드
    private TimeSpan _offlineElapsed; // 마지막 저장 이후 경과 시간

    /// <summary>
    /// 저장 데이터를 불러오고 게임을 시작한다.
    /// </summary>
    void Start()
    {
        SetGame();
    }

    /// <summary>
    /// 저장된 게임 상태와 보상을 적용하고 게임 진행을 시작한다.
    /// </summary>
    void SetGame()
    {
        _saveData = LoadData(); // 데이터를 로드하고
        ScoreManager.Instance.CurrentScore = _saveData.currentScore;
        HUDManager.Instance.UpdateScoreUI(_saveData.currentScore);
        _gameCoroutine = StartCoroutine(StartGameRoutine(StageManager.Instance.CurrentStage));
        if (_offlineReward > 0 && _offlineRewardPopup != null)
        {
            _offlineRewardPopup.Show(_offlineReward, _offlineElapsed);
        }
    }

    /// <summary>
    /// 저장 데이터를 복원하고 경과 시간에 따른 오프라인 보상을 지급한다.
    /// </summary>
    public SaveData LoadData()
    {
        _offlineReward = 0;
        _offlineElapsed = TimeSpan.Zero;
        if (!File.Exists(SaveManager.Instance.savePath))
        {
            Debug.Log("저장 파일이 없습니다.");
            return new SaveData();
        }

        string json = File.ReadAllText(SaveManager.Instance.savePath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        SetInfo(saveData);


        // 종료 시간을 불러와 리워드 적용
        if (!string.IsNullOrEmpty(saveData.lastSaveTime) && DateTime.TryParse(saveData.lastSaveTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastSaveTime))
        {
            TimeSpan elapsed = DateTime.UtcNow - lastSaveTime;
            if (elapsed.TotalSeconds >= 1)
            {
                _offlineElapsed = elapsed;
                _offlineReward = (long)elapsed.TotalSeconds;
                ScoreManager.Instance.GiveOfflineReward(saveData, _offlineReward);
            }
        }

        return saveData;

    }

    void SetInfo(SaveData saveData)
    {
        // 스테이지 적용
        StageManager.Instance.ChangeStage(saveData.currentStage);

        // 어택 레벨 적용
        AttackManager.Instance.SetAttackLevel(saveData.currentAttackLevel);

        // 어택 레벨 적용
        AbilityManager.Instance.SetAbilityLevel(saveData.currentAbilityLevel);

        // 드랍확률 레벨 적용
        DropManager.Instance.SetDropLevel(saveData.currentDropLevel);

        // 스킬 상태 불러오기
        SkillManager.Instance.InitializeSkills(saveData.skillDatas);

        // 스킬 패널 세팅
        ViewManager.Instance.menuView.SetSkillPanelUI(); 

        // 스킬 장착 상태 불러오기
        SkillManagement.Instance.LoadEquippedSkills(saveData.equippedSkillIDs);
    }


    /// <summary>
    /// 스테이지를 시작하고 클리어를 기다리는 게임 진행 루틴을 실행한다.
    /// </summary>
    IEnumerator StartGameRoutine(int stage)
    {
        Debug.Log("루틴");
        while (true)
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
