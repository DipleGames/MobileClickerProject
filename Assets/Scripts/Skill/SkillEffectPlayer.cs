using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffectPlayer : MonoBehaviour, IPoolable
{
    [SerializeField] private Animator _animator;

    private AnimatorOverrideController _overrideController;
    private Coroutine _returnCoroutine;
    private RectTransform _effectRect;

    private static readonly int _effectHash = Animator.StringToHash("SkillEffect");

    /// <summary>
    /// 이펙트의 화면 비율을 유지하고 애니메이션을 교체할 컨트롤러를 준비한다.
    /// </summary>
    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _effectRect = (RectTransform)transform;
        Image image = GetComponent<Image>();
        image.preserveAspect = true;
        image.raycastTarget = false;

        _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _animator.runtimeAnimatorController = _overrideController;
    }

    /// <summary>
    /// 몬스터 위에 스킬별 크기, 위치, 속도로 이펙트를 재생하고 풀에 반환한다.
    /// </summary>
    public void Play(SkillEffect effect)
    {
        AnimationClip clip = effect.effectClip;
        if (clip == null)
        {
            PoolManager.Instance.Return(this);
            return;
        }

        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
        }

        _overrideController["DefaultEffect"] = clip;

        _effectRect.sizeDelta = effect.EffectSize;
        _effectRect.anchoredPosition = effect.EffectOffset;
        _effectRect.SetAsFirstSibling();
        float speed = Mathf.Max(0.1f, effect.EffectSpeed);
        _animator.speed = speed;
        _animator.Play(_effectHash, 0, 0f);
        _animator.Update(0f);

        _returnCoroutine = StartCoroutine(ReturnAfterPlay(clip.length / speed));
    }

    private IEnumerator ReturnAfterPlay(float duration)
    {
        yield return new WaitForSeconds(duration);

        _returnCoroutine = null;
        PoolManager.Instance.Return<SkillEffectPlayer>(this);
    }

    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnReturnToPool()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }

        gameObject.SetActive(false);
    }
}
