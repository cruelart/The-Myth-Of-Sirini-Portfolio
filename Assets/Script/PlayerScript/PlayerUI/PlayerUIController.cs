using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerUIController : MonoBehaviour
{
    bool lockCamera = false;
    //private ITalkInteractable target; // 대화할 대상
    private PlayerInputReader playerInputReader; // 입력 담당

    private void Awake()
    {
        playerInputReader = GetComponent<PlayerInputReader>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UIManager.Instance.TalkUI.OnHide += HandleTalkHide;
        playerInputReader.OnInventoryPressed += InventoryToggle;
        playerInputReader.OnEquipSpacePressed += EquipSpaceToggle;
        playerInputReader.OnPlayerStatusPressed += PlayerStatusToggle;
        playerInputReader.OnQuestPressed += PlayerQuestToggle;
        playerInputReader.OnUIExitPressed += UIExit;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            GameEventChannel.OnLockCamera?.Invoke(!lockCamera);
            lockCamera = !lockCamera;
        }
    }

    private void ShowMouseButton()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideMouseButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    target = other.GetComponent<IInteractable>();
    //}

    //private void HandleTalkHide()
    //{
    //    target.ExitInteraction(transform.parent.gameObject);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    target = null;
    //}

    private void InventoryToggle()
    {
        switch (UIManager.Instance.IsOpenUI<InventoryUIManager>())
        {
            case true:
                HideMouseButton();
                UIManager.Instance.HideInventoryUI();
                break;

            case false:
                ShowMouseButton();
                UIManager.Instance.ShowInventoryUI();
                break;

        }
    }

    private void EquipSpaceToggle()
    {
        switch (UIManager.Instance.IsOpenUI<EquipSpaceUIManager>())
        {
            case true:
                HideMouseButton();
                UIManager.Instance.HideEquipSpaceUI();
                break;
            case false:
                ShowMouseButton();
                UIManager.Instance.ShowEquipSpaceUI();
                break;
        }
    }

    private void PlayerStatusToggle()
    {
        switch (UIManager.Instance.IsOpenUI<PlayerStatusUIManager>())
        {
            case true:
                HideMouseButton();
                UIManager.Instance.HidePlayerStatusUI();
                break;
            case false:
                ShowMouseButton();
                UIManager.Instance.ShowPlayerStatusUI();
                break;
        }
    }

    private void PlayerQuestToggle()
    {
        switch (UIManager.Instance.IsOpenUI<QuestUI>())
        {
            case true:
                HideMouseButton();
                UIManager.Instance.HideQuestUI();
                break;
            case false:
                ShowMouseButton();
                UIManager.Instance.ShowQuestUI();
                break;
        }
    }

    private void UIExit()
    {
        UIManager.Instance.InOrderUIHide();
    }
}
