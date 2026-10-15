using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    React1,
    Hit2,
    FrozenSkill1,
    FrozenSkill2,
    LevelUp,
}

public class EffectPoolManager : MonoBehaviour
{
    public static EffectPoolManager Instance;

    [SerializeField]
    private GameObject ReactEffectPrefab;

    [SerializeField]
    private GameObject FrozenSkill1EffectPrefab;

    [SerializeField]
    private GameObject FrozenSkill2EffectPrefab;

    [SerializeField]
    private GameObject LevelUpEffectPrefab;

    private Dictionary<EffectType, Queue<GameObject>> effectPoolMap = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        CreatePool(EffectType.React1, 30, ReactEffectPrefab);
        CreatePool(EffectType.LevelUp, 1, LevelUpEffectPrefab);
        CreatePool(EffectType.FrozenSkill1, 1, FrozenSkill1EffectPrefab);
        CreatePool(EffectType.FrozenSkill2, 1, FrozenSkill2EffectPrefab);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePool(EffectType _effectType, int _poolMaxNum, GameObject _prefab)
    {
        Queue<GameObject> pool = new Queue<GameObject>();

        effectPoolMap.Add(_effectType, pool);

        for(int i = 0; i < _poolMaxNum; i++)
        {
            GameObject effectObject = Instantiate(_prefab, transform);
            effectObject.SetActive(false);
            pool.Enqueue(effectObject);
        }
    }

    public void ReturnPool(EffectType _effectType, GameObject _pooledEffect)
    {
        _pooledEffect.SetActive(false);
        effectPoolMap[_effectType].Enqueue(_pooledEffect);
    }
    
    public void PlayEffect(EffectType _effectType, Vector3 _viewposition, Vector3 _viewrotation)
    {
        if(_viewrotation.sqrMagnitude < 0.001f)
        {
            _viewrotation = Vector3.forward;
        }

        Quaternion rotation = Quaternion.LookRotation(_viewrotation.normalized);

        var poolObj = effectPoolMap[_effectType].Dequeue();

        if(poolObj == null)
        {
            return;
        }

        PooledEffect pooledEffect = poolObj.GetComponent<PooledEffect>();
        pooledEffect.Init(_effectType);

        poolObj.SetActive(true);

        pooledEffect.Play( _viewposition, rotation);
    }

    public void MovePlayEffect(EffectType _effectType, Transform _transform, Vector3 _viewrotation)
    {
        if (_viewrotation.sqrMagnitude < 0.001f)
        {
            _viewrotation = Vector3.forward;
        }

        Quaternion rotation = Quaternion.LookRotation(_viewrotation.normalized);

        var poolObj = effectPoolMap[_effectType].Dequeue();

        if (poolObj == null)
        {
            return;
        }

        PooledEffect pooledEffect = poolObj.GetComponent<PooledEffect>();
        pooledEffect.Init(_effectType);

        poolObj.SetActive(true);

        pooledEffect.Play(_transform, rotation);
    }
}
