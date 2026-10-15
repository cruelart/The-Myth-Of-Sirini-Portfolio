using UnityEngine;
using UnityEngine.UIElements;

public class PooledEffect : MonoBehaviour
{
    private ParticleSystem root_particleSystem;

    private EffectType effectType;

    private bool isPlaying = false;

    private Transform targetTransform;

    private void Awake()
    {
        if (root_particleSystem == null)
        {
            root_particleSystem = GetComponentInChildren<ParticleSystem>(true);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        //모든 파티클 생존여부 확인
        if (root_particleSystem.IsAlive(true))
        {
            return;
        }

        isPlaying = false;
        EffectPoolManager.Instance.ReturnPool(effectType, this.gameObject);
    }

    private void FixedUpdate()
    {
        if(!isPlaying || targetTransform == null)
        {
            return;
        }

        transform.position = targetTransform.position;
    }

    private void OnDisable()
    {
        isPlaying = false;
        targetTransform = null;
    }

    public void Init(EffectType _effectType)
    {
        effectType = _effectType;
    }

    public void Play(Vector3 _position,Quaternion _rotation)
    {
        targetTransform = null;

        transform.SetPositionAndRotation(_position, _rotation);
        gameObject.SetActive(true);

        // 이전 풀에서의 흔적 제거
        root_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 
        root_particleSystem.Play(true);

        isPlaying = true;
    }

    public void Play(Transform _transformn, Quaternion _rotation)
    {
        transform.SetPositionAndRotation(_transformn.position, _rotation);
        targetTransform = _transformn;

        gameObject.SetActive(true);

        // 이전 풀에서의 흔적 제거
        root_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 
        root_particleSystem.Play(true);

        isPlaying = true;
    }


}
