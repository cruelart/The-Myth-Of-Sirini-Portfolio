using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class StatUI
{
    public StatType statType;
    public TextMeshProUGUI statValue_text;
}
public class PlayerStatusUIManager : UIBase
{
    private PlayerStatManager playerStatManager;

    [SerializeField]
    private List<StatUI> statList;

    private Dictionary<StatType, StatUI> statDict;

    [Header("플레이어 이름")]
    [SerializeField]
    private TextMeshProUGUI playerName_text;
    
    [Header("플레이어 레벨")]
    [SerializeField]
    private TextMeshProUGUI playerLV_text;

    [Header("업그레이드 가능 횟수")]
    [SerializeField]
    private TextMeshProUGUI possibleUpgradeValue_text;

    public void Init(PlayerStatManager _playerManager)
    {
        playerStatManager = _playerManager;
        playerStatManager.OnChangeStat += StatRefresh;
        statDict = statList.ToDictionary(s => s.statType);
        StatRefresh();
    }

    public void StatRefresh()
    {
        //이름 받아오기
        playerName_text.text = playerStatManager.GetPlayerName();

        //레벨 받아오기
        playerLV_text.text = "LV " + playerStatManager.GetPlayerLv().ToString();

        possibleUpgradeValue_text.text = playerStatManager.playerStatDB.stat_upgradePossibleValue.ToString();

        //스텟 받아오기
        foreach (var stat in statList)
        {
            stat.statValue_text.text = playerStatManager.GetStatValue(stat.statType).ToString("0.##");
        }
    }

    public void UpgradeStat(StatType _statType)
    {
        playerStatManager.UpgradeStatusStat(_statType, 1);

        //뭔가 여기서 스탯매니저쪽에서 힘을 올리면 공도 오르고 명중률도 올리고 하는 로직을 다 처리하고 난 뒤에

        StatRefresh(); // 전체적으로 다시 스탯받아오기
    }

    public void ClickStatPlusButton()
    {

    }
}
