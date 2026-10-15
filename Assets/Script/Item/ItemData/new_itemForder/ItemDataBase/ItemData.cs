using System.Collections.Generic;
using UnityEngine;

// 이 파일은 Json으로 부터 아이템을 받아올 용도로 쓸 예정
[System.Serializable]
public class ItemRawData
{
    //공용
    public int id;
    public string itemName;

    ////추가 옵션용
    //public List<Stat> stats = new List<Stat>();

    ////아이템 가격
    //public int sell_gold;
    //public int buy_gold;

    ////소비아이템용
    //public float waiting_time; // 재사용 대기시간
    //public float duration; // 지속시간
    //public float effectValue; // 적용 수치
    //public bool isDisposable; // 일회용인지 확인하는 변수
    //public bool isPercentage; // 퍼센트적용인지 확인하는 변수
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemRawData> items = new List<ItemRawData>();
}
