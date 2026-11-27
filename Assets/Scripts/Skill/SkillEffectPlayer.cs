using System.Collections;
using UnityEngine;

public class SkillEffectPlayer : MonoBehaviour, IPoolable
{
    [SerializeField] private Animator _animator;

    private AnimatorOverrideController _overrideController;
    private Coroutine _returnCoroutine;

    private static readonly int _effectHash = Animator.StringToHash("SkillEffect");

    /// <summary>
    /// 재생할 스킬 이펙트의 애니메이터와 오버라이드 컨트롤러를 준비한다.
    /// </summary>
    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _overrideController = new AnimatorOverrideController(
            _animator.runtimeAnimatorController
        );

        _animator.runtimeAnimatorController = _overrideController;
    }

    /// <summary>
    /// 지정한 이펙트 클립을 재생하고 재생 후 풀에 반환한다.
    /// </summary>
    public void Play(AnimationClip clip)
    {
        if (clip == null)
        {
            return;
        }

        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
        }

        _overrideController["DefaultEffect"] = clip;

        _animator.Play(_effectHash, 0, 0f);
        _animator.Update(0f);

        _returnCoroutine = StartCoroutine(ReturnAfterPlay(clip.length));
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