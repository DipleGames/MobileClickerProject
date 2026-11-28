using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager : SingleTon<PoolManager>
{
    [Header("데미지 텍스트")]
    [SerializeField] private DamageText _damageTextPrefab;
    [SerializeField] private Transform _damageTextParent;
    [SerializeField] private int _damageTextPoolSize = 32;

    [Header("추가 스코어 텍스트")]
    [SerializeField] private AdditionScoreText _additionTextPrefab;
    [SerializeField] private Transform _additionScoreTextParent;
    [SerializeField] private int _additionScoreTextPoolSize = 32;

    [Header("스킬 이펙트")]
    [SerializeField] private SkillEffectPlayer _skillEffectPlayerPrefab;
    [SerializeField] private Transform _skillEffectPlayerParent;
    [SerializeField] private int _skillEffectPlayerPoolSize = 4;

    [Header("기본공격 슬래쉬")]
    [SerializeField] private BasicAttackSlash _basicAttackSlash_Prefab;
    [SerializeField] private Transform _basicAttackSlashParent;
    [SerializeField] private int _basicAttackSlashPoolSize = 16;

    [Header("기본공격 임팩트")]
    [SerializeField] private BasicAttackImpact _basicAttackImpact_Prefab;
    [SerializeField] private Transform _basicAttackImpactParent;
    [SerializeField] private int _basicAttackImpactPoolSize = 16;

    [Header("정령 투사체")]
    [SerializeField] private SpiritProjectile _spiritProjectilePrefab; // 정령 투사체 풀에서 사용할 프리팹이다.
    [SerializeField] private Transform _spiritProjectileParent; // 정령 투사체를 생성할 정령 오브젝트다.
    [SerializeField] private int _spiritProjectilePoolSize = 4; // 미리 생성할 정령 투사체 수다.


    private readonly Dictionary<Type, object> _pools = new();
    /// <summary>
    /// 싱글턴을 초기화하고 각 프리팹의 오브젝트 풀을 등록한다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        Register(_damageTextPrefab, _damageTextParent, _damageTextPoolSize);
        Register(_additionTextPrefab, _additionScoreTextParent, _additionScoreTextPoolSize);
        Register(_skillEffectPlayerPrefab, _skillEffectPlayerParent, _skillEffectPlayerPoolSize);
        Register(_basicAttackSlash_Prefab, _basicAttackSlashParent, _basicAttackSlashPoolSize);
        Register(_basicAttackImpact_Prefab, _basicAttackImpactParent, _basicAttackImpactPoolSize);
        Register(_spiritProjectilePrefab, _spiritProjectileParent, _spiritProjectilePoolSize);
    }

    /// <summary>
    /// 프리팹과 부모를 지정해 해당 타입의 풀을 등록한다.
    /// </summary>
    private void Register<T>(T prefab, Transform parent, int poolSize) where T : MonoBehaviour, IPoolable
    {
        if (prefab == null)
        {
            Debug.LogError($"{typeof(T).Name} 풀의 프리팹이 지정되지 않았습니다.");
            return;
        }

        Type type = typeof(T);

        if (_pools.ContainsKey(type))
        {
            Debug.LogWarning($"{type.Name} 풀은 이미 등록되어 있습니다.");
            return;
        }

        ObjectPool<T> pool = new ObjectPool<T>(prefab, parent, poolSize);

        _pools.Add(type, pool);
    }

    public T Get<T>() where T : MonoBehaviour, IPoolable
    {
        Type type = typeof(T);

        if (!_pools.TryGetValue(type, out object poolObject))
        {
            Debug.LogError($"{type.Name} 풀이 등록되어 있지 않습니다.");
            return null;
        }

        if (poolObject is not ObjectPool<T> pool)
        {
            Debug.LogError($"{type.Name} 풀의 타입이 일치하지 않습니다.");
            return null;
        }

        return pool.Get();
    }

    public void Return<T>(T poolObject) where T : MonoBehaviour, IPoolable
    {
        if (poolObject == null)
        {
            Debug.LogWarning("반환하려는 오브젝트가 null입니다.");
            return;
        }

        Type type = typeof(T);

        if (!_pools.TryGetValue(type, out object registeredPool))
        {
            Debug.LogError($"{type.Name} 풀이 등록되어 있지 않습니다.");
            return;
        }

        if (registeredPool is not ObjectPool<T> pool)
        {
            Debug.LogError($"{type.Name} 풀의 타입이 일치하지 않습니다.");
            return;
        }

        pool.Return(poolObject);
    }

}
