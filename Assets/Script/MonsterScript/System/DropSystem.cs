using System.Collections.Generic;
using UnityEngine;

public static class DropSystem
{
    public static List<DropTableEntry> GetMonsterDropList(int _monsterID)
    {
        MonsterDB monsterDB = MonsterDBManager.Instance.GetMonsterDB(_monsterID); // 해당 몬스터의 데이터를 받아오고

        List<DropTableEntry> monsterDropList = new List<DropTableEntry>(); // 해당 몬스터가 떨굴 모든 아이템을 담는 리스트

        //공용 아이템 
        List<PublicDropTable> publicDropList = monsterDB.publicdropTables;

        //몬스터 개인의 드랍아이템
        List<DropTableEntry> personalDropList = monsterDB.personalDropItems;

        //공용아이템들 먼저 몰아놓기
        foreach(PublicDropTable monsterDropItem in publicDropList)
        {
            monsterDropList.AddRange(monsterDropItem.dropTableData);
        }

        //다 몰아넣었으면 개인용 아이템도 넣자
        monsterDropList.AddRange(personalDropList);

        return monsterDropList;
    }
}
