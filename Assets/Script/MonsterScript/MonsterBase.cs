using System;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public MonsterSpawnZone spawnZone;
}

//문제점
//-> OnEnable함수 호출에 있어서 유니티의 생명주기로 인해 오류발생
//-> MosnterToTalManager에서 start에서 호출했기때문에 OnEnable입장에서 stat이 비어있는데? 선언 따라서 이벤트 OnDeath가 정상적으로 HandleDeath를 구독하지못해 문제
