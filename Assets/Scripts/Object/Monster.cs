using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData currentMonsterData;

    [SerializeField] private AnimatorOverrideController _monsterAOC;
    [SerializeField] private int _monsterID;
    [SerializeField] private float _monsterHP;
    [FormerlySerializedAs("_currentmonsterHP")]
    [SerializeField] private float _currentMonsterHP;
    [SerializeField] private bool _isDie;

    [SerializeField] private Animator _anim;

    private RectTransform _rect;
    private Image _image;
    private Vector2 _restPosition;
    private Vector3 _restScale;
    private Color _restColor;
    private Sequence _hitReaction;
    private Sequence _hitFlash;
    private Sequence _hitScale;
    private float _nextHitAnimationTime;

    private static readonly int _hitHash = Animator.StringToHash("Hit");

    /// <summary>
    /// 피격 후 복귀할 몬스터의 위치와 색상을 저장한다.
    /// </summary>
    private void Awake()
    {
        _rect = (RectTransform)transform;
        _image = GetComponent<Image>();
        _restPosition = _rect.anchoredPosition;
        _restScale = _rect.localScale;
        _restColor = _image.color;
    }

    public float CurrentMonsterHP
    {
        get => _currentMonsterHP;
        set
        {
            _currentMonsterHP = Mathf.Max(0, value);
            if (_currentMonsterHP <= 0)
            {
                IsDie = true;
            }
        }
    }

    public bool IsDie
    {
        get => _isDie;
        set
        {
            if (_isDie == value)
            {
                return;
            }

            _isDie = value;

            if (_isDie)
            {
                StageManager.Instance.OnMonsterDied(true);
            }
        }
    }

    /// <summary>
    /// 몬스터 데이터로 체력과 애니메이션을 초기화하고 HUD를 갱신한다.
    /// </summary>
    public void InitMonster(MonsterData monsterData)
    {
        ResetHitReaction();
        currentMonsterData = monsterData;

        _monsterAOC = currentMonsterData.monsterAOC;
        _monsterID = currentMonsterData.monsterID;
        _monsterHP = currentMonsterData.monsterHP;
        _isDie = currentMonsterData.isDie;

        // Animator Controller 교체
        _anim.runtimeAnimatorController = _monsterAOC;

        // 이전 몬스터의 Hit 상태 초기화
        _anim.ResetTrigger("IsHit");

        // Idle 상태를 처음부터 재생
        _anim.Play("Idle", 0, 0f);
        _anim.Update(0f);

        CurrentMonsterHP = _monsterHP;

        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentMonsterHP, _monsterHP);
    }

    /// <summary>
    /// 피해를 적용하고 피격 애니메이션, 체력 UI, 데미지 텍스트를 갱신한다.
    /// </summary>
    public void TakeDamage(bool isSkill, double damage)
    {
        if (IsDie)
        {
            return;
        }

        float remainingHealth = _currentMonsterHP - (float)damage;
        ViewManager.Instance.gameView.ShowDamageText(isSkill, damage);
        CurrentMonsterHP = remainingHealth;
        if (remainingHealth > 0f)
        {
            PlayHitReaction();
            ViewManager.Instance.monsterView.UpdateMonsterHP(_currentMonsterHP, _monsterHP);
        }
    }

    /// <summary>
    /// 피격 애니메이션은 일정 간격으로 재생하고 매 타격의 짧은 반응은 부드럽게 갱신한다.
    /// </summary>
    private void PlayHitReaction()
    {
        if (Time.time >= _nextHitAnimationTime)
        {
            _anim.Play(_hitHash, 0, 0f);
            _nextHitAnimationTime = Time.time + 0.12f;
        }

        if (_hitReaction == null || !_hitReaction.IsActive())
        {
            float distance = currentMonsterData.isBoss ? 12f : 24f;
            _hitReaction = DOTween.Sequence().SetLink(gameObject);
            _hitReaction.Append(_rect.DOAnchorPosX(_restPosition.x + distance, 0.045f).SetEase(Ease.OutQuad));
            _hitReaction.Append(_rect.DOAnchorPosX(_restPosition.x, 0.14f).SetEase(Ease.OutCubic));
            _hitReaction.OnKill(() => _hitReaction = null);
        }

        _hitFlash?.Kill();
        Color hitColor = _restColor * new Color(1f, 0.68f, 0.62f, 1f);
        _hitFlash = DOTween.Sequence().SetLink(gameObject);
        _hitFlash.Append(_image.DOColor(hitColor, 0.025f));
        _hitFlash.Append(_image.DOColor(_restColor, 0.09f));
        _hitFlash.OnKill(() => _hitFlash = null);

        _hitScale?.Kill();
        float scaleMultiplier = currentMonsterData.isBoss ? 1.025f : 1.04f;
        _hitScale = DOTween.Sequence().SetLink(gameObject);
        _hitScale.Append(_rect.DOScale(_restScale * scaleMultiplier, 0.03f).SetEase(Ease.OutQuad));
        _hitScale.Append(_rect.DOScale(_restScale, 0.09f).SetEase(Ease.OutCubic));
        _hitScale.OnKill(() => _hitScale = null);
    }

    /// <summary>
    /// 이전 몬스터의 피격 상태를 초기화하고 전투 배치로 복귀시킨다.
    /// </summary>
    private void ResetHitReaction()
    {
        _hitReaction?.Kill();
        _hitFlash?.Kill();
        _hitScale?.Kill();
        _rect.anchoredPosition = _restPosition;
        _rect.localScale = _restScale;
        _image.color = _restColor;
        _nextHitAnimationTime = 0f;
    }
}
