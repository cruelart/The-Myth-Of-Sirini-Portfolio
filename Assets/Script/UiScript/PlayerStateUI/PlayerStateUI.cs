using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateUI : UIBase
{
    [SerializeField]
    private TextMeshProUGUI playerNameText;

    [SerializeField]
    private TextMeshProUGUI playerLvText;

    [SerializeField]
    private Image playerHpBarImage;

    [SerializeField]
    private Image playerMpBarImage;

    [SerializeField]
    private Image playerMentalBarImage;

    [SerializeField]
    private Image playerExpBarImage;

    private PlayerStatManager playerStatManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PlayerStatManager _playerStatManager)
    {
        playerStatManager = _playerStatManager;
        playerNameText.text = playerStatManager.GetPlayerName();
        playerLvText.text = playerStatManager.GetPlayerLv().ToString();

        playerHpBarImage.fillAmount = (float)(playerStatManager.playerStatDB.currentHp / playerStatManager.GetStatValue(StatType.HP));
        playerExpBarImage.fillAmount = (float)(playerStatManager.playerStatDB.currentExp / playerStatManager.maxExp);

        playerStatManager.OnHpChanged += ControlHpBar;
        playerStatManager.OnExpChanged += ControlExpBar;
        playerStatManager.OnLevelUp += ControlLevelText;
    }

    public void ControlHpBar(int _currentHp, int _maxHp)
    {
        playerHpBarImage.fillAmount = (float)_currentHp / _maxHp;
    }

    public void ControlLevelText(int _level)
    {
        playerLvText.text = _level.ToString();
    }

    public void ControllPlayerLvUI(int currentLv)
    {

    }

    public void ControlExpBar(long _currentExp, long _maxExp)
    {
        playerExpBarImage.fillAmount = (float)_currentExp / _maxExp;
    }

}
