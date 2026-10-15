using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITalkInteractable
{
    void Interact(Transform _interactor);
    void ExitInteraction();
}

public abstract class NpcTalkInteraction : MonoBehaviour, ITalkInteractable
{
    private NpcAIController NpcAIController; // 행동변화를 줘야되므로

    protected int textIndex = 0;

    private List<NpcTalkData> npcTalkDatas = new List<NpcTalkData>();

    //이벤트
    public event Action<Transform> OnNpcInteraction; // 상호작용 반응이 일어났음을 알림
    public event Action OffNpcInteraction; // 상호작용 반응이 끝났음을 알림

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(List<NpcTalkData> _npcTalkDatas)
    {
        npcTalkDatas = _npcTalkDatas;
    }

    public virtual void Interact(Transform _targetTransform)
    {
        DecideTextIndex();

        UIManager.Instance.ShowNpcTalkUI();
        UIManager.Instance.NpcTalkUI.Init(npcTalkDatas[textIndex]);

        OnNpcInteraction?.Invoke(_targetTransform);
    }

    public virtual void ExitInteraction()
    {
        OffNpcInteraction?.Invoke();
    }

    protected abstract void DecideTextIndex();
}
