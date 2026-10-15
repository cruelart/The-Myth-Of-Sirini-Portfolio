using System;
using UnityEngine;

public enum ActiveWeaponType
{
    None, // 무기 없음 -> 맨손
    MainWeapon, // 검같은거 -> 나중에 도끼같은것도 해볼까 생각중
    SecondaryWeapon // 총, 활등등? -> 일단 총만 구현
}

public class PlayerWeaponController : MonoBehaviour
{
    private EquipSpace equipSpace; // 장비창 ( 이게 있어야 무기 교체가능)
    private PlayerAnimationEvents animationEvents;

    public event Action<WeaponStyle> OnActiveWeaponStyleChanged;

    [SerializeField]
    private GameObject equip_sword;

    [SerializeField]
    private GameObject unequip_sword;

    // 현재 들고있는 무기 타입
    public ActiveWeaponType CurrentActiveSlot { get; private set; } = ActiveWeaponType.None;

    public bool IsWeaponDrawn =>  CurrentActiveSlot != ActiveWeaponType.None;

    // 현재 이동과 공격에서 사용할 자세
    public WeaponStyle CurrentWeaponStyle
    {
        get
        {
            if (CurrentActiveSlot == ActiveWeaponType.None)
            {
                return WeaponStyle.Unarmed;
            }

            WeaponStyle equippedStyle = GetEquippedWeaponStyle(CurrentActiveSlot);

            return equippedStyle == WeaponStyle.None
                ? WeaponStyle.Unarmed
                : equippedStyle;
        }
    }

    private void Awake()
    {
        equipSpace = GetComponent<EquipSpace>();
        animationEvents = GetComponent<PlayerAnimationEvents>();

        if (equipSpace == null)
        {
            Debug.LogError("장비창이 플레이어 오브젝트에 안붙어있음");
            enabled = false;
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (equipSpace == null)
        {
            return;
        }

        equipSpace.OnMainWeaponChanged += HandleMainWeaponChanged;
        equipSpace.OnSecondaryWeaponChanged += HandleSecondaryWeaponChanged;

        equipSpace.OnMainWeaponChanged += ViewMainWeapon;

        animationEvents.OnUnArmed += ViewUnArmed;
        animationEvents.OnEquipSword += ViewEquipSword;
    }
    private void OnDisable()
    {
        if (equipSpace == null)
        {
            return;
        }

        equipSpace.OnMainWeaponChanged -= HandleMainWeaponChanged;
        equipSpace.OnSecondaryWeaponChanged -= HandleSecondaryWeaponChanged;

        equipSpace.OnMainWeaponChanged -= ViewMainWeapon;

        animationEvents.OnUnArmed -= ViewUnArmed;
        animationEvents.OnEquipSword -= ViewEquipSword;
    }

    private void HandleMainWeaponChanged(WeaponStyle weaponStyle) // 예외 처리
    {
        if (weaponStyle == WeaponStyle.None && CurrentActiveSlot == ActiveWeaponType.MainWeapon) // 장비창에서 아이템 장착 해제했는데 해당 컨트롤러에서는 무기를 들고있다고 뜨면
        {
            ResetToUnarmed(); // 맨손으로 전환
        }
    }
    private void HandleSecondaryWeaponChanged(WeaponStyle weaponStyle)
    {
        if (weaponStyle == WeaponStyle.None && CurrentActiveSlot == ActiveWeaponType.SecondaryWeapon)
        {
            ResetToUnarmed();
        }
    }

    private void ResetToUnarmed() // 맨손격투 전환
    {
        ChangeActiveWeapon(ActiveWeaponType.None);
    }
    public WeaponStyle GetEquippedWeaponStyle(ActiveWeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case ActiveWeaponType.MainWeapon:
                if (equipSpace.CurrentMainWeapon == null)
                {
                    return WeaponStyle.None;
                }

                return equipSpace.CurrentMainWeapon.setting.weaponStyle;

            case ActiveWeaponType.SecondaryWeapon:
                if (equipSpace.CurrentSecondaryWeapon == null)
                {
                    return WeaponStyle.None;
                }

                return equipSpace.CurrentSecondaryWeapon.setting.weaponStyle;

            default:
                return WeaponStyle.None;
        }
    }

    //해당 무기가 실제로 장비창에 장착이 돼있는지 여부
    public bool IsWeaponEquipped(ActiveWeaponType weaponType)
    {
        switch (weaponType)
        {
            case ActiveWeaponType.MainWeapon:
                return equipSpace.CurrentMainWeapon != null;

            case ActiveWeaponType.SecondaryWeapon:
                return equipSpace.CurrentSecondaryWeapon != null;

            default:
                return false;
        }
    }

    //무기를 실제 든 상태 전환
    public bool CompleteDrawWeapon(ActiveWeaponType _weaponType)
    {
        if (CurrentActiveSlot != ActiveWeaponType.None)
        {
            return false;
        }

        if (!IsWeaponEquipped(_weaponType))
        {
            return false;
        }

        ChangeActiveWeapon(_weaponType);
        return true;
    }

    //무기를 든 상태 해제
    public bool CompleteSheatheWeapon()
    {
        if (CurrentActiveSlot == ActiveWeaponType.None)
        {
            return false;
        }

        ChangeActiveWeapon(ActiveWeaponType.None);
        return true;
    }
    private void ChangeActiveWeapon(ActiveWeaponType _activeWeaponType)
    {
        CurrentActiveSlot = _activeWeaponType;

        OnActiveWeaponStyleChanged?.Invoke(CurrentWeaponStyle);
    }

    public void ViewEquipSword()
    {
        equip_sword.SetActive(true);
        unequip_sword.SetActive(false);
    }

    public void ViewUnArmed()
    {
        equip_sword.SetActive(false);
        unequip_sword.SetActive(true);
    }

    public void ViewMainWeapon(WeaponStyle _weaponStyle)
    {
        switch(_weaponStyle)
        {
            case WeaponStyle.None:
                equip_sword.SetActive(false);
                unequip_sword.SetActive(false);
                break;

            case WeaponStyle.Sword:
                equip_sword.SetActive(false);
                unequip_sword.SetActive(true);
                break;
        }
    }
}
