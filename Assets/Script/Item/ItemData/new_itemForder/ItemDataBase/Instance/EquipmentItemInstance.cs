using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//장비 아이템 인스턴스 스크립트
public class EquipmentItemInstance
{
    public EquipmentItemDB setting;
    //public ItemRawData data; // Json에서 받아올 데이터 수치를 기록할거임
    public Dictionary<StatType, Stat> statDict = new Dictionary<StatType, Stat>();

    //생성 인스턴트아이템 데이터
    public long instanceID { get; private set; }

    //실시간 가변변수들은 인스턴스에서 관리
    public int upgradeLv { get; private set; } // 강화 단계
    public int durability { get; private set; } // 내구도

    //몬스터 드랍 용도
    public EquipmentItemInstance(EquipmentItemDB _EDB)
    {
        this.setting = _EDB;
        statDict = setting.stats.ToDictionary(s => s.type);
        //this.data = _raw;

        instanceID = ItemInstanceID.GetInstanceID();
        durability = UnityEngine.Random.Range(50, 101); // 50~ 100까지의 랜덤한 내구도 제공
        upgradeLv = 0;
    }

    //퀘스트지급이나 확정지급일 경우 내구도와 강화레벨 조절해서 주는 용도의 함수
    public EquipmentItemInstance(EquipmentItemDB _EDB, int _upgradeLv, int _durability)
    {
        this.setting = _EDB;
        //this.data = _raw;
        statDict = setting.stats.ToDictionary(s => s.type);

        instanceID = ItemInstanceID.GetInstanceID();
        upgradeLv = _upgradeLv;
        this.durability = _durability;
    }

    //장비 강화 함수
    public void Upgrade()
    {
        upgradeLv++; // 장비 레벨 상승
    }

    public float GetStat(StatType _type)
    {
        return statDict[_type].value + (upgradeLv * 5.0f);
    }


}
