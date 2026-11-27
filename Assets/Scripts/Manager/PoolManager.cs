using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager : SingleTon<PoolManager>
{
    [FormerlySerializedAs("_damage_Text_Prefab")]
    [Header("데미지 텍스트")]
    [SerializeField] private DamageText _damageTextPrefab;
    [FormerlySerializedAs("_damage_Text_Parent")]
    [SerializeField] private Transform _damageTextParent;
    [FormerlySerializedAs("_damage_Text_PoolSize")]
    [SerializeField] private int _damageTextPoolSize = 32;

    [FormerlySerializedAs("_addition_Text_Prefab")]
    [Header("추가 스코어 텍스트")]
    [SerializeField] private AdditionScoreText _additionTextPrefab;
    [FormerlySerializedAs("_additionScore_Text_Parent")]
    [SerializeField] private Transform _additionScoreTextParent;
    [FormerlySerializedAs("_additionScore_Text_PoolSize")]
    [SerializeField] private int _additionScoreTextPoolSize = 32;

    [FormerlySerializedAs("_skillEffectPlayer_Prefab")]
    [Header("스킬 이펙트")]
    [SerializeField] private SkillEffectPlayer _skillEffectPlayerPrefab;
    [FormerlySerializedAs("_skillEffectPlayer_Parent")]
    [SerializeField] private Transform _skillEffectPlayerParent;
    [FormerlySerializedAs("_skillEffectPlayer_PoolSize")]
    [SerializeField] private int _skillEffectPlayerPoolSize = 4;


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
    }

     private void Register<T>(T prefab, Transform parent, int poolSize) where T : MonoBehaviour, IPoolable
    {
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
