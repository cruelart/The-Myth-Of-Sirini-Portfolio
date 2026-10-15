using UnityEngine;

public class PlayerEffectSystem : MonoBehaviour
{
    private void Awake()
    {
        PlayerStatManager.Instance.OnLevelUp += (level) => ViewLevelUpEffect();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ViewLevelUpEffect()
    {
        EffectPoolManager.Instance.MovePlayEffect(EffectType.LevelUp, transform, new Vector3(0,0,0)); // 레벨업 이펙트 재생
    }
}
