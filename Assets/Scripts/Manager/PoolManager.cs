using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingleTon<PoolManager>
{
    [Header("데미지 텍스트")]
    [SerializeField] private DamageText _damage_Text_Prefab;
    [SerializeField] private Transform _damage_Text_Parent;
    [SerializeField] private int _damage_Text_PoolSize = 32;

    [Header("추가 스코어 텍스트")]
    [SerializeField] private AdditionScoreText _addition_Text_Prefab;
    [SerializeField] private Transform _additionScore_Text_Parent;
    [SerializeField] private int _additionScore_Text_PoolSize = 32;


    private readonly Dictionary<Type, object> _pools = new();
    protected override void Awake()
    {
        base.Awake();

        Register(_damage_Text_Prefab, _damage_Text_Parent, _damage_Text_PoolSize);
        Register(_addition_Text_Prefab, _additionScore_Text_Parent, _additionScore_Text_PoolSize);
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
