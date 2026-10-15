using UnityEngine;

//소비아이템 데이터베이스
[CreateAssetMenu(fileName = "ConsumerItemDB", menuName = "Scriptable Objects/Item/Consumer")]
public class ConsumerItemDB : BaseitemDB
{
    public float waiting_time; // 재사용 대기시간
    public float effectValue; // 적용 수치
    public float duration;
    public bool isDisposable; // 일회용인지 확인하는 변수
    public bool isPercentage; // 퍼센트적용인지 확인하는 변수
    public int maxNum; // 인벤토리에 들어갈 수 있는 해당 최대 소비아이템 갯수
}
